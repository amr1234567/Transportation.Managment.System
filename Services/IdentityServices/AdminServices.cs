using Core.Constants;
using Core.Dto.Identity;
using Core.Dto.UserInput;
using Core.Identity;
using ECommerce.Core.Interfaces.IServices;
using Interfaces.IIdentityServices;
using Microsoft.AspNetCore.Identity;
using System.Net.Mail;

namespace Services.IdentityServices
{
    public class AdminServices : IAdminServices
    {
        private readonly UserManager<ApplicationAdmin> _userManager;
        private readonly ITokenService _tokenService;

        public AdminServices(UserManager<ApplicationAdmin> userManager, ITokenService tokenService)
        {
            _userManager = userManager;
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

        public async Task<bool> SignUp(SignUpAsAdminDto NewUser)
        {
            if (NewUser == null)
                throw new ArgumentNullException("Model Can't Be null");

            var user = await _userManager.FindByEmailAsync(NewUser.Email);

            if (user != null)
                throw new Exception("Email Exist for another account");

            var appUser = new ApplicationAdmin
            {
                Email = NewUser.Email,
                Name = NewUser.Name,
                UserName = new MailAddress(NewUser.Email).User
            };

            var response = await _userManager.CreateAsync(appUser, NewUser.Password);
            if (!response.Succeeded)
                throw new Exception("Something went wrong");


            var User = await _userManager.FindByEmailAsync(appUser.Email);
            if (User == null)
                throw new Exception("Something went wrong");

            var res2 = await _userManager.AddToRoleAsync(User, Roles.Admin);
            if (!res2.Succeeded)
                throw new Exception($"Can't add the user for role {Roles.Admin}");

            return true;
        }
    }
}
