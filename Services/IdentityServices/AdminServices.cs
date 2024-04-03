using Core.Constants;
using Core.Dto.Identity;
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

        public async Task<bool> SignUp(SignUpDto NewUser)
        {
            if (NewUser == null)
                return false;
            var user = await _userManager.FindByEmailAsync(NewUser.Email);
            if (user != null)
                return false;

            var appUser = new ApplicationAdmin
            {
                Email = NewUser.Email,
                Name = NewUser.Name,
                UserName = new MailAddress(NewUser.Email).User,
                PhoneNumber = NewUser.PhoneNumber,
                PhoneNumberConfirmed = false,
            };
            var response = await _userManager.CreateAsync(appUser, NewUser.Password);
            if (!response.Succeeded)
                return false;


            var User = await _userManager.FindByEmailAsync(appUser.Email);
            if (User == null)
                return false;
            var res = await _userManager.SetPhoneNumberAsync(User, NewUser.PhoneNumber);

            if (!res.Succeeded)
                return false;

            var res2 = await _userManager.AddToRoleAsync(User, Roles.Admin);
            if (!res2.Succeeded)
                return false;
            return true;
        }
    }
}
