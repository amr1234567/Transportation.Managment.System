using Model.Dto;
using Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IServices.IServices
{
    public interface IJourneyServices
    {
        Task AddJourney(JourneyDto jouneyDto);
        Task SetArrivalTime(DateTime time, Guid id);
        Task SetleavingTime(DateTime time, Guid id);
        Task<List<Journey>> GetAllJouney();
        Task<List<Journey>> GetAllJouneyByBusStopId(Guid id);
        Task<Bus> GetNearestBusByDistenation(string distenationBusStop, string startBusStop);
        Task<bool> IsBusLeft(Guid id);
        Task<Journey> GetJourneyById(Guid id);
        Task ArrivedJourney(Guid id);
        Task LeftBus(Guid id);
    }
}
