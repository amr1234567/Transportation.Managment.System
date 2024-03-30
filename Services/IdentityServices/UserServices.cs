using Core.Constants;
using Core.Dto.Email;
using Core.Dto.Identity;
using Core.Identity;
using Interfaces.IIdentityServices;
using Interfaces.IMailServices;
using Microsoft.AspNetCore.Identity;
using System.Net.Mail;

namespace Services.IdentityServices
{
    public class UserServices : IUserServices
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMailServices _mailServices;

        public UserServices(
            UserManager<ApplicationUser> userManager,
            IMailServices mailServices
            )
        {
            _userManager = userManager;
            _mailServices = mailServices;
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

        public Task<LogInResponse> SignIn(LogInDto User)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SignUp(SignUpDto NewUser, string UrlToAction)
        {
            if (NewUser == null)
                return false;
            var user = await _userManager.FindByEmailAsync(NewUser.Email);
            if (user != null)
                return false;
            var appUser = new ApplicationUser
            {
                Email = NewUser.Email,
                Name = NewUser.Name,
                UserName = new MailAddress(NewUser.Email).User
            };
            var response = await _userManager.CreateAsync(appUser);
            if (!response.Succeeded)
                return false;
            var User = await _userManager.FindByEmailAsync(NewUser.Email);
            var res = await _userManager.AddToRoleAsync(User, Roles.User.ToString());
            if (!res.Succeeded)
                return false;
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(User);
            var url = UrlToAction + $"?token={token}&email={User.Email}";
            var message = new Message(
                new List<string> { User.Email },
                "Confirm Your Email",
                $"Click on the url : {url}"
                );
            _mailServices.SendEmail(message);
            return true;
        }
    }
}
