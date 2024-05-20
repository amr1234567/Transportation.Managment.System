using Core.Dto.Identity;
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
        Task<ResponseModel<string>> SignUp(SignUpDto NewUser);//, string Token);
        Task<ResponseModel<TokenModel>> SignIn(LogInDto User);
        Task<bool> ConfirmEmail(string Email, string Token);
        Task<bool> ConfirmPhoneNumber(string email, string PhoneNumber, string ConfirmToken);
        Task<ResponseModel<string>> ResetPasswordConfirmation(ResetPasswordDto model);
        Task<ResponseModel<string>> ResetPassword(string Email);
        Task<ResponseModel<bool>> VerifyChangePhoneNumber(string Email);
        Task<ResponseModel<bool>> ChangePhoneNumber(ChangePhoneNumberDto model);
        Task<ResponseModel<bool>> EditPersonalData(EditPersonalDataDto model, string userId);
    }
}
