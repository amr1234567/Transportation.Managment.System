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
                Tickets = journeyDto.ReservedTickets,
                Date = new DateTime(journeyDto.LeavingTime.Year, journeyDto.LeavingTime.Month, journeyDto.LeavingTime.Day)
            };

            _context.Journeys.Add(journey);
            _context.SaveChanges();
        }

        public Task<IEnumerable<JourneyHistory>> GetAllJourneys()
        {
            var Journeys = _context.Journeys
                                    .Include(j => j.Destination)
                                    .Include(j => j.StartBusStop)
                                    .Include(j => j.Tickets)
                                    .AsNoTracking().AsEnumerable();
            if (Journeys == null)
                throw new ArgumentNullException("No Journeys Right Now Check out Later");
            return Task.FromResult(Journeys);
        }

        public Task<JourneyHistory> GetJourneyById(Guid id)
        {

            var journey = _context.Journeys.FirstOrDefault(j => j.Id.CompareTo(id) == 0);
            if (journey == null)
                throw new ArgumentNullException("Journey Doesn't Exist");
            return Task.FromResult(journey);
        }
    }
}
