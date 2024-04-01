using Core.Dto;
using Core.Identity;
using Core.Models;
using Infrastructure.Context;
using Interfaces.IApplicationServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ApplicationServices
{
    public class JourneyServices : IJourneyServices
    {
        private readonly ApplicationDbContext _context;

        public JourneyServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddJourney(JourneyDto journeyDto) //wait
        {
            // Implement here bitch
            //create new journey and add it to DB

            var ticket = new List<Ticket>();
            var journey = new Journey()
            {
                //Id = Guid.NewGuid(),
                //Tickets = ticket,
                //DestinationName = journeyDto.Destination,
                //StartBusStop = journeyDto.StartBusStop,



            };
            await _context.Journeys.AddAsync(journey);
            await _context.SaveChangesAsync();
        }


        public async Task<List<Journey>> GetAllJourneysByBusStopId(Guid id) //done
        {
            //get all journeys in the bus stop with id "id" form Db and return it
            return await _context.Journeys.Where(x => x.StartBusStop.Id == id).ToListAsync();
        }

        public async Task<List<Journey>> GetAllJourneys() //done
        {
            // Implement here bitch
            //get all journeys form Db and return it
            return await _context.Journeys.ToListAsync();

        }


        public async Task<Journey> GetJourneyById(Guid id) //done
        {
            // Implement here bitch
            //get journey with id "id" and return it
            return await _context.Journeys.FindAsync(id);

        }

        public Task<Bus> GetNearestBusByDestination(string destinationBusStop, string startBusStop) // wait
        {
            // Implement here bitch

            //get all the journeys with the destination Bus Stop name "destinationBusStop" and start Bus Stop name "startBusStop"
            //and then return the first with the nearest time

            throw new NotImplementedException();
        }


        public async Task SetArrivalTime(DateTime time, Guid id)
        {
            // Implement here bitch

            //edit on journey with id "id" in field "ArrivalTime" (set it to "time" value)
            var journey = await _context.Journeys.FindAsync(id);
            journey.ArrivalTime = time;
            await _context.SaveChangesAsync();
        }

        public async Task SetLeavingTime(DateTime time, Guid id)
        {
            // Implement here bitch
            //edit on journey with id "id" in field "LeavingTime" (set it to "time" value)
            var journey = await _context.Journeys.FindAsync(id);
            journey.LeavingTime = time;
            await _context.SaveChangesAsync();
        }
    }
}
