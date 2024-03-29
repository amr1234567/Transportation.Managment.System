using Core.Dto;
using Core.Identity;
using Core.Models;
using Infrastructure.Context;
using Interfaces.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class JourneyServices : IJourneyServices
    {
        private readonly ApplicationDbContext _context;

        public JourneyServices(ApplicationDbContext context)
        {
            _context = context;
        }
        public Task AddJourney(JourneyDto journeyDto)
        {
            // Implement here bitch
            //create new journey and add it to DB
            throw new NotImplementedException();
        }

        public Task ArrivedJourney(Guid id)
        {
            // Implement here bitch
            //Edit on journey in arrived field to set it as arrived
            throw new NotImplementedException();
        }

        public Task<List<Journey>> GetAllJourneysByBusStopId(Guid id)
        {
            //get all journeys in the bus stop with id "id" form Db and return it
            throw new NotImplementedException();
        }

        public Task<List<Journey>> GetAllJourneys()
        {
            // Implement here bitch
            //get all journeys form Db and return it
            throw new NotImplementedException();
        }


        public Task<Journey> GetJourneyById(Guid id)
        {
            // Implement here bitch
            //get journey with id "id" and return it
            throw new NotImplementedException();
        }

        public Task<Bus> GetNearestBusByDestination(string destinationBusStop, string startBusStop)
        {
            // Implement here bitch

            //get all the journeys with the destination Bus Stop name "destinationBusStop" and start Bus Stop name "startBusStop"
            //and then return the first with the nearest time

            throw new NotImplementedException();
        }

        public Task<bool> IsBusLeft(Guid id)
        {
            // Implement here bitch
            //return true if journey.isleft == true 
            throw new NotImplementedException();
        }

        public Task LeftBus(Guid id)
        {
            // Implement here bitch
            //edit on journey with id "id" in field "isleft" to true

            throw new NotImplementedException();
        }

        public Task SetArrivalTime(DateTime time, Guid id)
        {
            // Implement here bitch

            //edit on journey with id "id" in field "ArrivalTime" (set it to "time" value)
            throw new NotImplementedException();
        }

        public Task SetLeavingTime(DateTime time, Guid id)
        {
            // Implement here bitch
            //edit on journey with id "id" in field "LeavingTime" (set it to "time" value)

            throw new NotImplementedException();
        }
    }
}
