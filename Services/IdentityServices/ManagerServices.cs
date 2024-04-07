using Core.Constants;
using Core.Dto.Identity;
using Core.Dto.UserInput;
using Core.Dto.UserOutput;
using Core.Identity;
using ECommerce.Core.Interfaces.IServices;
using Infrastructure.Context;
using Interfaces.IApplicationServices;
using Interfaces.IIdentityServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;

namespace Services.IdentityServices
{
    public class ManagerServices : IManagerServices
    {
        private readonly UserManager<BusStopManger> _userManager;
        private readonly ITokenService _tokenService;
        private readonly ApplicationDbContext _context;

        public ManagerServices(UserManager<BusStopManger> userManager, ITokenService tokenService, ApplicationDbContext context)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _context = context;
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

            var NewManager = new BusStopManger
            {
                Email = NewUser.Email,
                Name = NewUser.Name,
                UserName = new MailAddress(NewUser.Email).User,
                PhoneNumber = NewUser.PhoneNumber
            };

            var response = await _userManager.CreateAsync(NewManager, NewUser.Password);
            if (!response.Succeeded)
                return false;


            var Manager = await _userManager.FindByEmailAsync(NewManager.Email);
            if (Manager == null)
                return false;



            await _userManager.UpdateAsync(Manager);

            var res = await _userManager.SetPhoneNumberAsync(Manager, NewUser.PhoneNumber);

            if (!res.Succeeded)
                return false;

            var res2 = await _userManager.AddToRoleAsync(Manager, Roles.BusStopManager);
            if (!res2.Succeeded)
                return false;
            return true;
        }
        public async Task enrollBusStop(string Id, string busStopId)
        {
            var record = await _context.BusStopMangers.Include(bs => bs.BusStops)
                                        .FirstOrDefaultAsync(bs => bs.Id.Equals(Id));

            var record2 = await _context.BusStopMangers.Include(bs => bs.BusStops)
                                        .FirstOrDefaultAsync(bs => bs.Id.Equals(busStopId));

            record.BusStops.Add(record2);

            await _context.SaveChangesAsync();
        }

        public async Task<List<BusStopDto>> GetAllBusStops()
        {
            return _context.BusStopMangers.Select(x => new BusStopDto { Name = x.Name }).ToList();
        }

        public async Task<BusStopDto> GetBusStop(string Id)
        {
            var record = await _context.BusStopMangers.Include(bsm => bsm.BusStops).FirstOrDefaultAsync(bsm => bsm.Id == Id);
            return new BusStopDto
            {
                Name = record.Name,
                BusStops = record.BusStops.Select(x => new BusStopDto { Name = x.Name }).ToList()
            };
        }
    }
}
