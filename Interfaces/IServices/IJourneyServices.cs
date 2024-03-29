using Core.Dto;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.IServices
{
    public interface IJourneyServices
    {
        Task AddJourney(JourneyDto jouneyDto);
        Task SetArrivalTime(DateTime time, Guid id);
        Task SetLeavingTime(DateTime time, Guid id);
        Task<List<Journey>> GetAllJourneys();
        Task<List<Journey>> GetAllJourneysByBusStopId(Guid id);
        Task<Bus> GetNearestBusByDestination(string distenationBusStop, string startBusStop);
        Task<bool> IsBusLeft(Guid id);
        Task<Journey> GetJourneyById(Guid id);
        Task ArrivedJourney(Guid id);
        Task LeftBus(Guid id);
    }
}
