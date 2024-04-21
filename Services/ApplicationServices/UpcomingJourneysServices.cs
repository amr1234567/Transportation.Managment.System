using Core.Dto;
using Core.Dto.ServiceInput;
using Core.Dto.UserOutput;
using Core.Models;
using Infrastructure.Context;
using Interfaces.IApplicationServices;
using Microsoft.EntityFrameworkCore;

namespace Services.ApplicationServices
{
    public class UpcomingJourneysServices(ISeatServices seatServices, ApplicationDbContext context, IJourneysHistoryServices journeyHistoryServices) : IUpcomingJourneysServices
    {
        private readonly ISeatServices _seatServices = seatServices;
        private readonly ApplicationDbContext _context = context;
        private readonly IJourneysHistoryServices _journeyHistoryServices = journeyHistoryServices;

        public Task<IEnumerable<ReturnedUpcomingJourneyDto>> GetAllUpcomingJourneys()
        {
            var records = _context.UpcomingJourneys
                .Include(x => x.Destination)
                .Include(y => y.StartBusStop)
                .Select(x =>
                     new ReturnedUpcomingJourneyDto
                     {
                         ArrivalTime = x.ArrivalTime,
                         DestinationName = x.Destination.Name,
                         LeavingTime = x.LeavingTime,
                         NumberOfAvailableTickets = _context.Buses.FirstOrDefault(b => b.Id.Equals(x.BusId)).seats.Count(s => s.IsAvailable),
                         StartBusStopName = x.StartBusStop.Name,
                         TicketPrice = x.TicketPrice,
                         BusId = x.BusId,
                         DestinationId = x.Destination.Id,
                         Id = x.Id,
                         JourneyId = x.JourneyId,
                         StartBusStopId = x.StartBusStopId
                     }).AsNoTracking().AsEnumerable();

            if (records == null)
                throw new ArgumentNullException("No Upcoming Journeys");

            return Task.FromResult(records);
        }

        public void TurnUpcomingJourneysIntoHistoryJourneys()
        {
            var records = _context.UpcomingJourneys.Where(x => x.ArrivalTime < DateTime.UtcNow).ToList();

            if (records == null)
                throw new ArgumentNullException("No Journey exist");

            foreach (var record in records)
            {
                _journeyHistoryServices.AddJourney(new JourneyDto
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
                if (bus == null)
                    throw new ArgumentNullException("Bus Can't be null");
                bus.IsAvailable = true;

                var seats = _context.Seats.Where(x => x.BusId.Equals(record.BusId));

                if (seats == null)
                    throw new ArgumentNullException("Seats Can't be null");

                foreach (var seat in seats)
                    seat.IsAvailable = true;
                _context.UpcomingJourneys.Remove(record);
            }
            _context.SaveChanges();
        }

        public async Task<UpcomingJourneyDto> AddUpcomingJourney(UpcomingJourneyDto model)
        {
            if (model == null)
                throw new ArgumentNullException("Journey Details can't be null");

            var BusStop = await _context.BusStopMangers.Include(bsm => bsm.BusStops)
                .FirstOrDefaultAsync(bsm => bsm.Id.Equals(model.StartBusStopId));
            if (BusStop == null)
                throw new NullReferenceException("StartBus Stop Can't be null");
            var BusStopExist = BusStop.BusStops.Any(bsm => bsm.Id.CompareTo(model.DestinationId) == 0);
            if (!BusStopExist)
                throw new NullReferenceException("Destination BusStop Can't be null");
            var NumberOfAvailableTickets = _context.Buses.Include(b => b.seats).FirstOrDefault(b => b.Id.Equals(model.BusId)).seats.Count(s => s.IsAvailable);

            var TimeTable = new UpcomingJourney
            {
                ArrivalTime = model.ArrivalTime,
                BusId = model.BusId,
                DestinationId = model.DestinationId,
                StartBusStopId = model.StartBusStopId,
                TicketPrice = model.TicketPrice,
                LeavingTime = model.LeavingTime,
                NumberOfAvailableTickets = NumberOfAvailableTickets,
                JourneyId = Guid.NewGuid(),
            };

            await _context.UpcomingJourneys.AddAsync(TimeTable);
            var bus = _context.Buses.Find(model.BusId);
            bus.IsAvailable = false;
            await _context.SaveChangesAsync();
            return model;
        }

        public Task<IEnumerable<ReturnedUpcomingJourneyDto>> GetAllJourneysByStartBusStopId(string id)
        {
            if (id == null)
                throw new ArgumentNullException("Start BusStopId Can't Be Null");

            var journeys = _context.UpcomingJourneys.Where(x => x.StartBusStopId.Equals(id));
            var ReturnedJourneys = journeys
                .Select(x => new ReturnedUpcomingJourneyDto
                {
                    ArrivalTime = x.ArrivalTime,
                    DestinationName = x.Destination.Name,
                    LeavingTime = x.LeavingTime,
                    NumberOfAvailableTickets = _context.Buses.FirstOrDefault(b => b.Id.Equals(x.BusId)).seats.Count(s => s.IsAvailable),
                    StartBusStopName = x.StartBusStop.Name,
                    TicketPrice = x.TicketPrice,
                    BusId = x.BusId,
                    DestinationId = x.Destination.Id,
                    Id = x.Id,
                    JourneyId = x.JourneyId,
                    StartBusStopId = x.StartBusStopId
                }).AsNoTracking().AsEnumerable();
            if (journeys == null)
                throw new ArgumentNullException($"Journey With StartBusStop: {id} doesn't exist");
            return Task.FromResult(ReturnedJourneys);
        }

        public Task<IEnumerable<ReturnedUpcomingJourneyDto>> GetAllJourneysByDestinationBusStopId(string id)
        {
            if (id == null)
                throw new ArgumentNullException("Destination BusStopId Can't Be Null");
            var journeys = _context.UpcomingJourneys.Where(x => x.DestinationId.Equals(id));
            if (journeys == null)
                throw new ArgumentNullException($"Journey With Destination BusStop: {id} doesn't exist");
            var returnedJourneys = journeys.Select(j => new ReturnedUpcomingJourneyDto
            {
                ArrivalTime = j.ArrivalTime,
                BusId = j.BusId,
                DestinationId = j.DestinationId,
                DestinationName = j.DestinationName,
                Id = j.Id,
                JourneyId = j.JourneyId,
                LeavingTime = j.LeavingTime,
                NumberOfAvailableTickets = j.NumberOfAvailableTickets,
                Seats = _context.Seats.Where(s => s.BusId.Equals(j.BusId)),
                StartBusStopId = j.StartBusStopId,
                StartBusStopName = j.StartBusStopName,
                TicketPrice = j.TicketPrice,
            }).AsNoTracking().AsEnumerable();
            return Task.FromResult(returnedJourneys);
        }

        public async Task<ReturnedUpcomingJourneyDto> GetJourneyById(Guid id)
        {
            if (id == null)
                throw new ArgumentNullException("Journey Can't Be Null");
            var journey = await _context.UpcomingJourneys.FirstOrDefaultAsync(x => x.Id.Equals(id));
            if (journey == null)
                throw new ArgumentNullException($"Journy With {id} doesn't exist");
            var seats = await _seatServices.GetAllSeatsInBusByBusId(journey.BusId);
            return new ReturnedUpcomingJourneyDto
            {
                StartBusStopId = journey.StartBusStopId,
                NumberOfAvailableTickets = journey.NumberOfAvailableTickets,
                DestinationName = journey.DestinationName,
                LeavingTime = journey.LeavingTime,
                StartBusStopName = journey.StartBusStopName,
                TicketPrice = journey.TicketPrice,
                JourneyId = journey.JourneyId,
                ArrivalTime = journey.ArrivalTime,
                BusId = journey.BusId,
                Id = journey.Id,
                DestinationId = journey.DestinationId,
                Seats = seats,
            };
        }


        public async Task<ReturnedUpcomingJourneyDto> GetNearestJourneyByDestination(string destinationId, string startBusStopId) // wait
        {
            var Journeys = _context.UpcomingJourneys.Where(j => j.StartBusStopId.Equals(startBusStopId)
                                        && j.DestinationId.Equals(destinationId));

            if (Journeys is null || !Journeys.Any())
                throw new Exception("No Buses");

            var JourneysCounted = Journeys.OrderBy(b => b.LeavingTime);
            var journey = JourneysCounted.First();
            var seats = await _seatServices.GetAllSeatsInBusByBusId(journey.BusId);
            return new ReturnedUpcomingJourneyDto
            {
                StartBusStopId = journey.StartBusStopId,
                NumberOfAvailableTickets = journey.NumberOfAvailableTickets,
                DestinationName = journey.DestinationName,
                LeavingTime = journey.LeavingTime,
                StartBusStopName = journey.StartBusStopName,
                TicketPrice = journey.TicketPrice,
                JourneyId = journey.JourneyId,
                ArrivalTime = journey.ArrivalTime,
                BusId = journey.BusId,
                Id = journey.Id,
                DestinationId = journey.DestinationId,
                Seats = seats
            };
        }


        public async Task SetArrivalTime(DateTime time, Guid id)
        {
            if (id == null)
                throw new ArgumentNullException("Journey Id Can't Be Null");

            var journey = await GetJourneyById(id);
            if (journey is null)
                throw new NullReferenceException($"Journey With Id: {id} Doesn't Exist");

            journey.ArrivalTime = time;
            await _context.SaveChangesAsync();
        }

        public async Task SetLeavingTime(DateTime time, Guid id)
        {

            if (id == null)
                throw new ArgumentNullException("Journey Id Can't Be Null");

            var journey = await GetJourneyById(id);
            if (journey is null)
                throw new NullReferenceException($"Journey With Id: {id} Doesn't Exist");

            journey.LeavingTime = time;
            await _context.SaveChangesAsync();
        }
    }
}
