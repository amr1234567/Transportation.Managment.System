﻿using Core.Constants;
using Core.Dto;
using Core.Dto.Identity;
using Core.Identity;
using ECommerce.Core.Interfaces.IServices;
using Interfaces.IApplicationServices;
using Interfaces.IIdentityServices;
using Microsoft.AspNetCore.Identity;
using System.Net.Mail;

namespace Services.IdentityServices
{
    public class ManagerServices : IManagerServices
    {
        private readonly UserManager<BusStopManger> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IBusStopServices _busStopServices;

        public ManagerServices(UserManager<BusStopManger> userManager, ITokenService tokenService, IBusStopServices busStopServices)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _busStopServices = busStopServices;
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

            var busStop = await _busStopServices.AddBusStop(new BusStopDto
            {
                Name = NewManager.Name
            }, Manager.Id);

            Manager.BusStop = busStop;
            await _userManager.UpdateAsync(Manager);

            var res = await _userManager.SetPhoneNumberAsync(Manager, NewUser.PhoneNumber);

            if (!res.Succeeded)
                return false;

            var res2 = await _userManager.AddToRoleAsync(Manager, Roles.BusStopManager);
            if (!res2.Succeeded)
                return false;
            return true;
        }
    }
}
