using Core.Dto.ServiceInput;
using Core.Models;
using Infrastructure.Context;
using Interfaces.IApplicationServices;
using Microsoft.EntityFrameworkCore;

namespace Services.ApplicationServices
{
    public class JourneysHistoryServices(ApplicationDbContext context) : IJourneysHistoryServices
    {
        private readonly ApplicationDbContext _context = context;

        public void AddJourney(JourneyDto journeyDto) //wait
        {
            if (journeyDto == null)
                throw new ArgumentNullException("Journey Details Can't be null");
            var journey = new JourneyHistory()
            {
                Id = journeyDto.Id,
                DestinationId = journeyDto.DestinationId.ToString(),
                StartBusStopId = journeyDto.StartNusStopId.ToString(),
                BusId = journeyDto.BusId,
                ArrivalTime = journeyDto.ArrivalTime,
                LeavingTime = journeyDto.LeavingTime,
                Tickets = journeyDto.ReservedTickets
            };

            _context.Journeys.Add(journey);
            _context.SaveChanges();
        }

        public async Task<List<JourneyHistory>> GetAllJourneys()
        {
            var Journeys = await _context.Journeys
                                    .Include(j => j.Destination)
                                    .Include(j => j.StartBusStop)
                                    .Include(j => j.Tickets)
                                    .ToListAsync();
            if (Journeys == null)
                throw new ArgumentNullException("No Journeys Right Now Check out Later");
            return Journeys;
        }



        public async Task<JourneyHistory> GetJourneyById(Guid id)
        {

            var journey = _context.Journeys.FirstOrDefault(j => j.Id.CompareTo(id) == 0);
            if (journey == null)
                throw new ArgumentNullException("Journey Doesn't Exist");
            return journey;
        }


    }
}
