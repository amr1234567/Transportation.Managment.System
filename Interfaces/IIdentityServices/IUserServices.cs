using Core.Dto.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.IIdentityServices
{
    public interface IUserServices
    {
        Task<bool> SignUp(SignUpDto NewUser, string Token);
        Task<LogInResponse> SignIn(LogInDto User);
        Task<bool> ConfirmEmail(string Email, string Token);
    }
}
