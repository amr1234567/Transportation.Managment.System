using Core.Dto.Identity;

namespace Interfaces.IIdentityServices
{
    public interface IAdminServices
    {
        Task<bool> SignUp(SignUpDto NewUser);//, string Token);
        Task<LogInResponse> SignIn(LogInDto User);
    }
}
