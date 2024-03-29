using IServices.IServices;
using Model.Dto;
using Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class JourneyServices : IJourneyServices
    {

        public Task AddJourney(JourneyDto jouneyDto)
        {
            // Implement here bitch
            throw new NotImplementedException();
        }

        public Task ArrivedJourney(Guid id)
        {
            // Implement here bitch
            throw new NotImplementedException();
        }

        public Task<List<Journey>> GetAllJouney()
        {
            // Implement here bitch
            throw new NotImplementedException();
        }

        public Task<List<Journey>> GetAllJouneyByBusStopId(Guid id)
        {
            // Implement here bitch
            throw new NotImplementedException();
        }

        public Task<Journey> GetJourneyById(Guid id)
        {
            // Implement here bitch
            throw new NotImplementedException();
        }

        public Task<Bus> GetNearestBusByDistenation(string distenationBusStop, string startBusStop)
        {
            // Implement here bitch
            throw new NotImplementedException();
        }

        public Task<bool> IsBusLeft(Guid id)
        {
            // Implement here bitch
            throw new NotImplementedException();
        }

        public Task LeftBus(Guid id)
        {
            // Implement here bitch
            throw new NotImplementedException();
        }

        public Task SetArrivalTime(DateTime time, Guid id)
        {
            // Implement here bitch
            throw new NotImplementedException();
        }

        public Task SetleavingTime(DateTime time, Guid id)
        {
            // Implement here bitch
            throw new NotImplementedException();
        }
    }
}
