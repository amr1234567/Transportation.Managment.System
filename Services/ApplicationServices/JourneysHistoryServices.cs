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

        public async Task<List<JourneyHistory>> GetAllJourneys() =>
            await _context.Journeys.Include(j => j.Bus)
                                    .Include(j => j.Destination)
                                    .Include(j => j.StartBusStop)
                                    .ToListAsync();



        public async Task<JourneyHistory> GetJourneyById(Guid id)
        {
            var journeys = await GetAllJourneys();
            return journeys.FirstOrDefault(j => j.Id.CompareTo(id) == 0);
        }


    }
}
