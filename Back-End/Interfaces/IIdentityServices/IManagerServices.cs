using Core.Dto.Identity;
using Core.Dto.UserInput;
using Core.Dto.UserOutput;

namespace Interfaces.IIdentityServices
{
    public interface IManagerServices
    {
        Task<bool> SignUp(SignUpAsManagerDto model);//, string Token);
        Task<ResponseModel<TokenModel>> SignIn(LogInDto model);
        Task<IEnumerable<ReturnedBusStopDto>> GetAllStartBusStops();
        Task<IEnumerable<ReturnedBusStopDto>> GetAllDestinationBusStops(string StartBusStopId);
        Task enrollBusStop(string StartBusStopId, string DestinationBusStopId);
        Task<ReturnedBusStopDto> GetBusStop(string Id);
    }
}
