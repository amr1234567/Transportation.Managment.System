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
        Task<ResponseModel<bool>> SignUp(SignUpDto NewUser);//, string Token);
        Task<LogInResponse> SignIn(LogInDto User);
        Task<bool> ConfirmEmail(string Email, string Token);
        Task<bool> ConfirmPhoneNumber(string PhoneNumber, string UserCode);
        Task<ResponseModel<string>> ResetPasswordConfirmation(ResetPasswordDto model);
        Task<ResponseModel<bool>> ResetPassword(string PhoneNumber);
        Task<ResponseModel<bool>> VerifyChangePhoneNumber(string PhoneNumber);
        Task<ResponseModel<bool>> ChangePhoneNumber(string Verifytoken, string PhoneNumber);
        Task<ResponseModel<bool>> EditPersonalData(EditPersonalDataDto model, string userId);
    }
}
