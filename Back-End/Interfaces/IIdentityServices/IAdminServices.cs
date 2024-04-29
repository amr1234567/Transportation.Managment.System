using Core.Dto.Identity;
using Core.Dto.UserInput;

namespace Interfaces.IIdentityServices
{
    public interface IAdminServices
    {
        Task<bool> SignUp(SignUpAsAdminDto NewUser);//, string Token);
        Task<LogInResponse> SignIn(LogInDto User);
    }
}
