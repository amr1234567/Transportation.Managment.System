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

        public async Task<bool> SignUp(SignUpAsManagerDto NewUser)
        {
            if (NewUser == null)
                throw new ArgumentNullException("Model Can't Be null");

            var user = await _userManager.FindByEmailAsync(NewUser.Email);

            if (user != null)
                throw new Exception("Email Exist for another account");

            var NewManager = new BusStopManger
            {
                Email = NewUser.Email,
                Name = NewUser.Name,
                UserName = new MailAddress(NewUser.Email).User
            };

            var response = await _userManager.CreateAsync(NewManager, NewUser.Password);
            if (!response.Succeeded)
                throw new Exception("Something went wrong");


            var User = await _userManager.FindByEmailAsync(NewManager.Email);
            if (User == null)
                throw new Exception("Something went wrong");


            var res2 = await _userManager.AddToRoleAsync(User, Roles.BusStopManager);
            if (!res2.Succeeded)
                throw new Exception($"Can't add the user for role {Roles.BusStopManager}");

            return true;

        }
        public async Task enrollBusStop(string StartBusStopId, string DestrnationBusStopId)
        {
            var record = await _context.BusStopMangers.Include(bs => bs.BusStops)
                                        .FirstOrDefaultAsync(bs => bs.Id.Equals(StartBusStopId));

            if (record == null || record.BusStops is null)
                throw new NullReferenceException($"Bus stop with Id {StartBusStopId} Doesn't Exist");

            var record2 = await _context.BusStopMangers.Include(bs => bs.BusStops)
                                        .FirstOrDefaultAsync(bs => bs.Id.Equals(DestrnationBusStopId));

            if (record2 == null || record2.BusStops is null)
                throw new NullReferenceException($"Bus stop with Id {DestrnationBusStopId} Doesn't Exist");

            record.BusStops = [.. record.BusStops, record2];
            record2.BusStops = [.. record2.BusStops, record];

            await _context.SaveChangesAsync();
        }

        public Task<IEnumerable<ReturnedBusStopDto>> GetAllBusStops()
        {
            var busStops = _context.BusStopMangers.Include(bs => bs.BusStops)
                .Select(x => new ReturnedBusStopDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    busStops = x.BusStops.Select(x => new ReturnedBusStopDto { Id = x.Id, Name = x.Name }),
                }).AsNoTracking().AsEnumerable();
            return Task.FromResult(busStops);
        }

        public async Task<ReturnedBusStopDto> GetBusStop(string Id)
        {
            var record = await _context.BusStopMangers.Include(bsm => bsm.BusStops)
                .FirstOrDefaultAsync(bsm => bsm.Id == Id);

            if (record == null)
                throw new NullReferenceException($"Bus stop with Id {Id} Doesn't Exist");

            return new ReturnedBusStopDto
            {
                Id = record.Id,
                Name = record.Name,
                busStops = record.BusStops.Select(x => new ReturnedBusStopDto { Id = x.Id, Name = x.Name })
            };
        }
    }
}
