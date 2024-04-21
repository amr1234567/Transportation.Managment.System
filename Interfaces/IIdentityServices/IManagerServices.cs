﻿using Core.Dto.Identity;
using Core.Dto.UserInput;
using Core.Dto.UserOutput;

namespace Interfaces.IIdentityServices
{
    public interface IManagerServices
    {
        Task<bool> SignUp(SignUpAsManagerDto NewUser);//, string Token);
        Task<LogInResponse> SignIn(LogInDto User);
        Task<IEnumerable<ReturnedBusStopDto>> GetAllBusStops();
        Task enrollBusStop(string Id, string busStopId);
        Task<ReturnedBusStopDto> GetBusStop(string Id);
    }
}
