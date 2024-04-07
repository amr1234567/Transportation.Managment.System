using Core.Dto;
using Core.Dto.ServiceInput;
using Core.Models;
using Infrastructure.Context;
using Interfaces.IApplicationServices;
using Microsoft.EntityFrameworkCore;

namespace Services.ApplicationServices
{
    public class UpcomingJourneysServices : IUpcomingJourneysServices
    {
        private readonly ApplicationDbContext _context;
        private readonly IJourneysHistoryServices _journeyServices;
        public UpcomingJourneysServices(ApplicationDbContext context, IJourneysHistoryServices journeyServices)
        {

            _context = context;
            _journeyServices = journeyServices;
        }
        public async Task<List<ReturnTimeTableDto>> GetAllUpcomingJourneys()
        {
            var records = await _context.UpcomingJourneys
                .Include(x => x.Destination)
                .Include(y => y.StartBusStop)
                .Select(x =>
                new ReturnTimeTableDto
                {
                    ArrivalTime = x.ArrivalTime,
                    DestinationName = x.Destination.Name,
                    LeavingTime = x.LeavingTime,
                    NumberOfAvailableTickets = _context.Buses.FirstOrDefault(b => b.Id.Equals(x.BusId)).seats.Count(s => s.IsAvailable),
                    StartBusStopName = x.StartBusStop.Name,
                    TicketPrice = x.TicketPrice
                }).ToListAsync();

            return records;
        }

        public void RemoveUpcomingJourneys()
        {
            var records = _context.UpcomingJourneys.Where(x => x.ArrivalTime < DateTime.UtcNow).ToList();
            foreach (var record in records)
            {
                _journeyServices.AddJourney(new JourneyDto
                {
                    Id = record.Id,
                    ArrivalTime = record.ArrivalTime,
                    BusId = record.BusId,
                    DestinationId = record.DestinationId,
                    LeavingTime = record.LeavingTime,
                    ReservedTickets = record.Ticket,
                    StartNusStopId = record.StartBusStopId
                });

                var bus = _context.Buses.Find(record.BusId);
                bus.IsAvailable = true;
                var seats = _context.Seats.Where(x => x.BusId.Equals(record.BusId));

                foreach (var seat in seats)
                    seat.IsAvailable = true;
                _context.UpcomingJourneys.Remove(record);
            }
            _context.SaveChanges();
        }

        public async Task<TimeTableDto> SetUpcomingJourneys(TimeTableDto time)
        {
            var BusStop = await _context.BusStopMangers.Include(bsm => bsm.BusStops)
                .FirstOrDefaultAsync(bsm => bsm.Id.Equals(time.StartBusStopId));
            if (BusStop == null)
                throw new NullReferenceException("");
            var BusStopExist = BusStop.BusStops.Any(bsm => bsm.Id.Equals(time.DestinationId));
            if (!BusStopExist)
                throw new NullReferenceException("");

            var TimeTable = new UpcomingJourney
            {
                ArrivalTime = time.ArrivalTime,
                BusId = time.BusId,
                DestinationId = time.DestinationId,
                StartBusStopId = time.StartBusStopId,
                TicketPrice = time.TicketPrice,
                LeavingTime = time.LeavingTime,
                NumberOfAvailableTickets = _context.Buses.FirstOrDefault(b => b.Id.Equals(time.BusId)).seats.Count(s => s.IsAvailable),
                JourneyId = Guid.NewGuid(),
            };

            await _context.UpcomingJourneys.AddAsync(TimeTable);
            var bus = _context.Buses.Find(time.BusId);
            bus.IsAvailable = false;
            await _context.SaveChangesAsync();
            return time;
        }

        public async Task<List<UpcomingJourney>> GetAllJourneysByStartBusStopId(string id)
        {
            var journeys = await _context.UpcomingJourneys.Where(x => x.StartBusStopId.Equals(id)).ToListAsync();
            return journeys;
        }

        public async Task<List<UpcomingJourney>> GetAllJourneysByDestinationBusStopId(string id)
        {
            var journeys = await _context.UpcomingJourneys.Where(x => x.DestinationId.Equals(id)).ToListAsync();
            return journeys;
        }

        public async Task<UpcomingJourney> GetJourneyById(Guid id)
        {
            var journey = await _context.UpcomingJourneys.FirstOrDefaultAsync(x => x.Id.Equals(id));
            return journey;
        }


        public async Task<UpcomingJourney> GetNearestJourneyByDestination(string destinationId, string startBusStopId) // wait
        {
            var Journeys = await _context.UpcomingJourneys.Where(j => j.StartBusStopId.Equals(startBusStopId)
                                        && j.DestinationId.Equals(destinationId)).ToListAsync();

            if (Journeys is null || !Journeys.Any())
                throw new Exception("No Buses");

            var JourneysCounted = Journeys.OrderBy(b => b.LeavingTime);

            return JourneysCounted.First();
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
