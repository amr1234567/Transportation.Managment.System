using Core.Constants;
using Core.Dto.Email;
using Core.Dto.Identity;
using Core.Dto.UserInput;
using Core.Dto.UserOutput;
using Core.Helpers;
using Core.Identity;
using ECommerce.Core.Interfaces.IServices;
using Interfaces.IHelpersServices;
using Interfaces.IIdentityServices;
using Interfaces.IMailServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Services.HelperServices;
using System.Net.Mail;
using System.Runtime;
using Twilio.Jwt.AccessToken;
using Twilio.Rest.Verify.V2.Service;
using Twilio.Types;

namespace Services.IdentityServices
{
    public class UserServices : IUserServices
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMailServices _mailServices;
        private readonly ISmsSevices _smsSevices;
        private readonly ITokenService _tokenService;

        public UserServices(
            UserManager<ApplicationUser> userManager,
            IMailServices mailServices,
            ISmsSevices smsSevices,
            ITokenService tokenService
            )
        {
            _userManager = userManager;
            _mailServices = mailServices;
            _smsSevices = smsSevices;
            _tokenService = tokenService;
        }

        public async Task<bool> ConfirmEmail(string Email, string Token)
        {
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Token))
                throw new ArgumentNullException("Input Can't be null");

            var user = await _userManager.FindByNameAsync(Email);
            if (user == null)
                throw new NullReferenceException($"Account with Email {Email} Not Found");

            var response = await _userManager.ConfirmEmailAsync(user, Token);
            if (!response.Succeeded)
                throw new Exception($"Something went wrong");

            return true;
        }

        public async Task<bool> ConfirmPhoneNumber(string PhoneNumber, string DBCode, string UserCode)
        {
            if (string.IsNullOrEmpty(PhoneNumber) || string.IsNullOrEmpty(DBCode) || string.IsNullOrEmpty(UserCode))
                throw new ArgumentNullException("Input Can't be null");

            #region  With VerificationCheckResource
            //var verification = await VerificationCheckResource.CreateAsync(
            //      to: PhoneNumber,
            //      code: Code,
            //      pathServiceSid: _TwilioSettings.Value.VerificationServiceSID
            //  );

            //if (verification.Status == "approved")
            //{
            //    var identityUser = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber.Equals(PhoneNumber));
            //    identityUser.PhoneNumberConfirmed = true;
            //    var updateResult = await _userManager.UpdateAsync(identityUser);

            //    if (updateResult.Succeeded)
            //    {
            //        return true;
            //    }
            //}
            //return false; 
            #endregion
            if (!DBCode.Equals(UserCode))
                throw new Exception("Two Codes are not equal");

            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber.Equals(PhoneNumber));
            if (user == null)
                throw new NullReferenceException($"Phone Number is Wrong");

            user.PhoneNumberConfirmed = true;
            var response = await _userManager.UpdateAsync(user);

            if (!response.Succeeded)
                throw new Exception($"Something went wrong");

            return true;
        }

        public async Task<LogInResponse> SignIn(LogInDto User)
        {
            if (User == null)
                throw new ArgumentNullException("Model Can't Be null");

            var user = await _userManager.FindByEmailAsync(User.Email);
            if (user == null)
                throw new NullReferenceException("Email or Password Wrong");

            var check = await _userManager.CheckPasswordAsync(user, User.Password);
            if (!check)
                throw new NullReferenceException("Email or Password Wrong");

            var userRoles = await _userManager.GetRolesAsync(user);
            var token = await _tokenService.CreateToken(user, userRoles.ToList());
            return new LogInResponse()
            {
                StatusCode = 200,
                TokenModel = token,
                Message = "Logged In"
            };
        }

        public async Task<string> SignUp(SignUpDto NewUser)//, string UrlToAction)
        {
            if (NewUser == null)
                throw new ArgumentNullException("Model Can't Be null");

            var user = await _userManager.FindByEmailAsync(NewUser.Email);

            if (user != null)
                throw new Exception("Email Exist for another account");

            var appUser = new ApplicationUser
            {
                Email = NewUser.Email,
                Name = NewUser.Name,
                UserName = new MailAddress(NewUser.Email).User,
                PhoneNumber = NewUser.PhoneNumber
            };

            var response = await _userManager.CreateAsync(appUser, NewUser.Password);
            if (!response.Succeeded)
                throw new Exception("Something went wrong");

            var User = await _userManager.FindByEmailAsync(appUser.Email);
            if (User == null)
                throw new Exception("Something went wrong");

            var res = await _userManager.SetPhoneNumberAsync(User, NewUser.PhoneNumber);

            if (!res.Succeeded)
                throw new Exception("Can't Save Phone Number");


            #region Add Role

            var res2 = await _userManager.AddToRoleAsync(User, Roles.User);
            if (!res2.Succeeded)
                throw new Exception($"Can't add the user for role {Roles.User}");

            #endregion

            #region Custom Code
            var code = _smsSevices.GenerateCode();
            if (string.IsNullOrEmpty(code))
                throw new Exception("Something went wrong in GenerateCode Function");

            var _ = _smsSevices.Send($"Verification Code : {code}", appUser.PhoneNumber);

            return code;

            #endregion

            #region Phone Verification First Try
            //var verification = await VerificationResource.CreateAsync(
            //    to: appUser.PhoneNumber,
            //    channel: "sms",
            //    pathServiceSid: _TwilioSettings.Value.VerificationServiceSID
            //   );

            //if (verification.Status == "pending")
            //{
            //    return true;
            //}
            //return false; 
            #endregion

            #region Email Verification
            //---------------------------------------------------------------------
            //var token = await _userManager.GenerateEmailConfirmationTokenAsync(User);
            //var url = UrlToAction + $"?token={token}&email={User.Email}";
            //var message = new Message(
            //    new List<string> { User.Email },
            //    "Confirm Your Email",
            //    $"Click on the url : {url}"
            //    );
            //_mailServices.SendEmail(message); 
            #endregion
            //return code;
        }
        public async Task<string> ResetPassword(string PnoneNumber)
        {
            var user = _userManager.Users.FirstOrDefault(x => x.PhoneNumber == PnoneNumber);
            if (user == null)
                throw new NullReferenceException($"Phone Number isn't correct");

            var code = _smsSevices.GenerateCode();

            if (string.IsNullOrEmpty(code))
                throw new Exception("Something went wrong in GenerateCode Function");

            var _ = _smsSevices.Send($"Verification Code : {code}", PnoneNumber);

            return code;
        }


        public async Task<ResponseModel<string>> ResetPasswordConfirmation(ResetPasswordDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.email);
            if (user == null)
                throw new NullReferenceException("Email Doesn't Exist");
            if (model.Realcode != model.code)
                throw new Exception("Verification Wrong");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            if (string.IsNullOrEmpty(token))
                throw new Exception("Something went wrong in GenerateCode Function");

            var result = await _userManager.ResetPasswordAsync(user, token, model.password);
            if (!result.Succeeded)
                throw new Exception("Can't reset Password");

            return new ResponseModel<string>
            {
                Message = "Code sent to phoneNumber",
                StatusCode = 200
            };

        }
    }
}
