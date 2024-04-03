using Core.Dto;
using Core.Models;
using Infrastructure.Context;
using Interfaces.IApplicationServices;
using Microsoft.EntityFrameworkCore;

namespace Services.ApplicationServices
{
    public class JourneyServices : IJourneyServices
    {
        private readonly ApplicationDbContext _context;

        public JourneyServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddJourney(JourneyDto journeyDto, Guid DestinationId, Guid StartNusStopId, Guid BusId) //wait
        {
            var journey = new Journey()
            {
                Id = Guid.NewGuid(),
                DestinationId = DestinationId,
                StartBusStopId = StartNusStopId,
                BusId = BusId,
                ArrivalTime = journeyDto.ArrivalTime,
                LeavingTime = journeyDto.LeavingTime
            };

            await _context.Journeys.AddAsync(journey);
            await _context.SaveChangesAsync();
        }


        public async Task<List<Journey>> GetAllJourneysByBusStopId(Guid id) =>
             await _context.Journeys.Where(x => x.StartBusStop.Id.Equals(id)).ToListAsync();


        public async Task<List<Journey>> GetAllJourneys() =>
            await _context.Journeys.ToListAsync();



        public async Task<Journey> GetJourneyById(Guid id) =>
             await _context.Journeys.FirstOrDefaultAsync(j => j.Id.Equals(id));

        public async Task<Journey> GetNearestJourneyByDestination(Guid destinationId, Guid startBusStopId) // wait
        {
            var buses = _context.Journeys.Where(j => j.StartBusStopId.Equals(startBusStopId) && j.DestinationId.Equals(destinationId));
            if (buses is null || !buses.Any())
                throw new Exception("No Buses");
            var busesCounted = buses.OrderBy(b => b.LeavingTime);
            return await busesCounted.FirstAsync();
        }


        public async Task SetArrivalTime(DateTime time, Guid id)
        {
            var journey = await GetJourneyById(id);
            if (journey is null)
                throw new NullReferenceException(nameof(journey));
            journey.ArrivalTime = time;
            await _context.SaveChangesAsync();
        }

        public async Task SetLeavingTime(DateTime time, Guid id)
        {
            var journey = await GetJourneyById(id);
            if (journey is null)
                throw new NullReferenceException(nameof(journey));
            journey.LeavingTime = time;
            await _context.SaveChangesAsync();
        }
    }
}
