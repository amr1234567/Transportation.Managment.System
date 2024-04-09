﻿using Core.Dto.Identity;
using Core.Dto.UserInput;
using Core.Dto.UserOutput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.IIdentityServices
{
    public interface IUserServices
    {
        Task<string> SignUp(SignUpDto NewUser);//, string Token);
        Task<LogInResponse> SignIn(LogInDto User);
        Task<bool> ConfirmEmail(string Email, string Token);
        Task<bool> ConfirmPhoneNumber(string PhoneNumber, string DBCode, string UserCode);
        Task<ResponseModel<string>> ResetPasswordConfirmation(ResetPasswordDto model);
        Task<string> ResetPassword(string PnoneNumber);
    }
}
