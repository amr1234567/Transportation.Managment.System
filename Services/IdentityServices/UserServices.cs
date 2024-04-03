using Core.Constants;
using Core.Dto;
using Core.Dto.Email;
using Core.Dto.Identity;
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
            var user = await _userManager.FindByNameAsync(Email);
            if (user == null)
                return false;
            var response = await _userManager.ConfirmEmailAsync(user, Token);
            if (!response.Succeeded)
                return false;
            return true;
        }

        public async Task<bool> ConfirmPhoneNumber(string PhoneNumber, string DBCode, string UserCode)
        {
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
                return false;
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber.Equals(PhoneNumber));
            if (user == null)
                return false;

            user.PhoneNumberConfirmed = true;
            var response = await _userManager.UpdateAsync(user);

            if (!response.Succeeded)
                return false;
            return true;
        }

        public async Task<LogInResponse> SignIn(LogInDto User)
        {
            if (User == null)
                return new LogInResponse()
                {
                    StatusCode = 400,
                    Message = "Model is Empty"
                };
            var user = await _userManager.FindByEmailAsync(User.Email);
            if (user == null)
                return new LogInResponse()
                {
                    StatusCode = 404,
                    Message = "Wrong Email Or Password"
                };
            var check = await _userManager.CheckPasswordAsync(user, User.Password);
            if (!check)
                return new LogInResponse()
                {
                    StatusCode = 404,
                    Message = "Wrong Email Or Password"
                };
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
                return string.Empty;
            var user = await _userManager.FindByEmailAsync(NewUser.Email);
            if (user != null)
                return string.Empty;
            var appUser = new ApplicationUser
            {
                Email = NewUser.Email,
                Name = NewUser.Name,
                UserName = new MailAddress(NewUser.Email).User,
                PhoneNumber = NewUser.PhoneNumber
            };
            var response = await _userManager.CreateAsync(appUser, NewUser.Password);
            if (!response.Succeeded)
                return string.Empty;

            var User = await _userManager.FindByEmailAsync(appUser.Email);

            var res = await _userManager.SetPhoneNumberAsync(User, NewUser.PhoneNumber);

            if (!res.Succeeded)
                return string.Empty;

            #region Add Role

            if (User == null)
                return string.Empty;
            var res2 = await _userManager.AddToRoleAsync(User, Roles.User);
            if (!res2.Succeeded)
                return string.Empty;

            #endregion

            #region Custom Code
            var code = _smsSevices.GenerateCode();
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
            if (user == null) return string.Empty;
            var code = _smsSevices.GenerateCode();
            var _ = _smsSevices.Send($"Verification Code : {code}", PnoneNumber);
            return code;
        }


        public async Task<ResponseModel<string>> ResetPasswordConfirmation(string code, string password, string email, string Realcode)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                throw new Exception();
            if (Realcode != code)
                throw new Exception();

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, password);
            return new ResponseModel<string>
            {
                Message = "Code sent to phoneNumber",
                StatusCode = 200
            };

        }
    }
}
