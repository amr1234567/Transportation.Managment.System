﻿using Core.Constants;
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



        public async Task<ResponseModel<bool>> SignUp(SignUpDto NewUser)//, string UrlToAction)
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
            //var code = _smsSevices.GenerateCode();
            //if (string.IsNullOrEmpty(code))
            //    throw new Exception("Something went wrong in GenerateCode Function");
            var code = await _userManager.GenerateChangePhoneNumberTokenAsync(User, NewUser.PhoneNumber);
            var result = _smsSevices.Send($"Verification Code : {code}", appUser.PhoneNumber);

            if (result.Status == Twilio.Rest.Api.V2010.Account.MessageResource.StatusEnum.Accepted)
                return new ResponseModel<bool>
                {
                    StatusCode = 200,
                    Message = "Registering operation succeeded, please confirm your email",
                    Body = true
                };
            else
                return new ResponseModel<bool>
                {
                    StatusCode = 500,
                    Message = "SomeThing Went Wrong, Please Try Again"
                };
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

        public async Task<bool> ConfirmEmail(string Email, string Token)
        {
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Token))
                throw new ArgumentNullException("Input Can't be null");

            var user = await _userManager.FindByEmailAsync(Email);
            if (user == null)
                throw new NullReferenceException($"Account with Email {Email} Not Found");

            var response = await _userManager.ConfirmEmailAsync(user, Token);
            if (!response.Succeeded)
                throw new Exception($"Something went wrong");

            return true;
        }

        public async Task<bool> ConfirmPhoneNumber(string PhoneNumber, string ConfirmToken)
        {
            if (string.IsNullOrEmpty(PhoneNumber) || string.IsNullOrEmpty(ConfirmToken))
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

            var user = await FindUserByPhoneNumberAsync(PhoneNumber);

            var result = await _userManager.VerifyChangePhoneNumberTokenAsync(user, ConfirmToken, PhoneNumber);
            if (result)
            {
                user.PhoneNumberConfirmed = true;
                var response = await _userManager.UpdateAsync(user);

                if (!response.Succeeded)
                    throw new Exception($"Something went wrong");
            }
            else
                throw new Exception($"Verification Code is wrong");
            return result;
        }



        public async Task<ResponseModel<bool>> ResetPassword(string PhoneNumber)
        {
            var user = await FindUserByPhoneNumberAsync(PhoneNumber);

            //var code = _smsSevices.GenerateCode();

            //if (string.IsNullOrEmpty(code))
            //    throw new Exception("Something went wrong in GenerateCode Function");
            var code = await _userManager.GenerateChangePhoneNumberTokenAsync(user, PhoneNumber);
            var result = _smsSevices.Send($"Verification Code : {code}", PhoneNumber);

            if (result.Status == Twilio.Rest.Api.V2010.Account.MessageResource.StatusEnum.Accepted)
                return new ResponseModel<bool>
                {
                    StatusCode = 200,
                    Body = true,
                    Message = "Verify Message Sent"
                };
            else
                return new ResponseModel<bool>
                {
                    StatusCode = 500,
                    Message = "SomeThing Went Wrong, Please Try Again"
                };

        }

        public async Task<ResponseModel<string>> ResetPasswordConfirmation(ResetPasswordDto model)
        {
            var user = await FindUserByPhoneNumberAsync(model.phoneNumber);

            var response = await _userManager.VerifyChangePhoneNumberTokenAsync(user, model.code, model.phoneNumber);
            if (!response)
                throw new Exception("Verification Code is Wrong");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            if (string.IsNullOrEmpty(token))
                throw new Exception("Something went wrong in Generate Code Service");

            var result = await _userManager.ResetPasswordAsync(user, token, model.password);
            if (!result.Succeeded)
                throw new Exception("Can't reset Password");

            return new ResponseModel<string>
            {
                Message = "Password Reseted Successfully",
                StatusCode = 200
            };

        }



        public async Task<ResponseModel<bool>> EditPersonalData(EditPersonalDataDto model, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new NullReferenceException($"user with id '{userId}' can't be found");
            user.Email = string.IsNullOrEmpty(model.Email) ? user.Email : model.Email;
            user.Name = string.IsNullOrEmpty(model.Name) ? user.Name : model.Name;
            var result = await _userManager.UpdateAsync(user);
            return (result.Succeeded) ?
                new ResponseModel<bool>
                {
                    StatusCode = 200,
                    Message = "Data Edited Successfully"
                } : new ResponseModel<bool>
                {
                    StatusCode = 500,
                    Message = "Something Went Wrong"
                };
        }



        public async Task<ResponseModel<bool>> VerifyChangePhoneNumber(string PhoneNumber)
        {
            var user = await FindUserByPhoneNumberAsync(PhoneNumber);
            var code = await _userManager.GenerateChangePhoneNumberTokenAsync(user, PhoneNumber);
            var result = _smsSevices.Send($"Verification Code : {code}", PhoneNumber);

            if (result.Status == Twilio.Rest.Api.V2010.Account.MessageResource.StatusEnum.Accepted)
                return new ResponseModel<bool>
                {
                    StatusCode = 200,
                    Message = "Registering operation succeeded, please confirm your email"
                };
            else
                return new ResponseModel<bool>
                {
                    StatusCode = 500,
                    Message = "SomeThing Went Wrong, Please Try Again"
                };
        }

        public async Task<ResponseModel<bool>> ChangePhoneNumber(string VerificationToken, string PhoneNumber)
        {
            var user = await FindUserByPhoneNumberAsync(PhoneNumber);
            var result = await _userManager.VerifyChangePhoneNumberTokenAsync(user, VerificationToken, PhoneNumber);
            if (!result)
                throw new Exception("Verification Code is Wrong");
            user.PhoneNumber = PhoneNumber;
            var response = await _userManager.UpdateAsync(user);
            return response.Succeeded ? new ResponseModel<bool>
            {
                StatusCode = 200,
                Body = true,
                Message = "Change your phone number has been done successfully"
            } : new ResponseModel<bool>
            {
                StatusCode = 500,
                Message = "Something went wrong while change your password, please try again"
            };
        }



        private async Task<ApplicationUser> FindUserByPhoneNumberAsync(string PhoneNumber)
        {
            if (string.IsNullOrEmpty(PhoneNumber))
                throw new ArgumentNullException("Input Can't be null");
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == PhoneNumber);
            if (user == null)
                throw new NullReferenceException($"user with phone number '{PhoneNumber}' doesn't exist");
            return user;
        }
    }
}