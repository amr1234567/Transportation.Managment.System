using Core.Dto.UserInput;
using Core.Dto.UserOutput;
using Core.Identity;
using Core.Models;
using Infrastructure.Context;
using Interfaces.IApplicationServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Services.ApplicationServices
{
    public class TicketServices(ApplicationDbContext context, ISeatServices seatServices) : ITicketServices
    {
        private readonly ApplicationDbContext _context = context;
        private readonly ISeatServices _seatServices = seatServices;

        private async Task GenerateTicket(TicketDto ticketDto, string ConsumerId, bool Online)
        {
            if (ticketDto == null)
                throw new ArgumentNullException("Ticket Data Can't Be Null");
            if (ConsumerId == null)
                throw new ArgumentNullException("UnAuthorized");


            var seatNum = _context.Seats.Find(Guid.Parse(ticketDto.SeatId)).SeatNum;

            if (seatNum == null)
                throw new ArgumentNullException("Seat Doesn't Exist");

            var journey = await _context.UpcomingJourneys.Include(j => j.Destination)
                                            .Include(j => j.StartBusStop)
                                            .FirstOrDefaultAsync(j => j.Id.Equals(Guid.Parse(ticketDto.JourneyId)));
            if (journey == null)
                throw new NullReferenceException("journey can't be Found");


            var ticket = new Ticket()
            {
                Id = Guid.NewGuid(),
                CreatedTime = DateTime.UtcNow,
                SeatNum = seatNum,
                UpcomingJourneyId = Guid.Parse(ticketDto.JourneyId),
                ConsumerId = ConsumerId,
                ReservedOnline = Online,
                Price = journey.TicketPrice,
                JourneyId = Guid.Parse(ticketDto.JourneyId),
                ArrivalTime = journey.ArrivalTime,
                DestinationId = journey.DestinationId,
                DestinationName = journey.Destination.Name,
                LeavingTime = journey.LeavingTime,
                StartBusStopId = journey.StartBusStopId,
                StartBusStopName = journey.StartBusStop.Name
            };

            await _seatServices.ReserveSeat(Guid.Parse(ticketDto.SeatId));
            await _context.Tickets.AddAsync(ticket);

            await _context.SaveChangesAsync();
        }

        public async Task<ResponseModel<bool>> CutTicket(TicketDto ticketDto, string ConsumerId)
        {
            if (ticketDto == null)
                throw new ArgumentNullException("Ticket Data Can't Be Null");
            if (ConsumerId == null)
                throw new ArgumentNullException("UnAuthorized");

            await GenerateTicket(ticketDto, ConsumerId, false);
            return new ResponseModel<bool>
            {
                StatusCode = 200,
                Message = "Ticket Cut Successfully"
            };
        }

        public async Task<ResponseModel<bool>> BookTicket(TicketDto ticketDto, string UserId)
        {

            if (ticketDto == null)
                throw new ArgumentNullException("Ticket Data Can't Be Null");
            if (UserId == null)
                throw new ArgumentNullException("UnAuthorized");

            await GenerateTicket(ticketDto, UserId, true);
            return new ResponseModel<bool>
            {
                StatusCode = 200,
                Message = "Ticket Booked Successfully"
            };
        }

        public Task<IEnumerable<ReturnedTicketDto>> GetAllTickets()
        {
            var tickets = _context.Tickets.Select(t => new ReturnedTicketDto
            {
                ArrivalTime = t.ArrivalTime,
                LeavingTime = t.LeavingTime,
                DestinationBusStopName = t.DestinationName,
                JourneyId = t.JourneyId,
                Price = t.Price,
                SeatNumber = t.SeatNum,
                StartBusStopName = t.StartBusStopName
            }).AsNoTracking().AsEnumerable();
            if (tickets == null)
                throw new ArgumentNullException("No Tickets Exist");

            return Task.FromResult(tickets);
        }


        public Task<IEnumerable<ReturnedTicketDto>> GetAllTicketsByJourneyId(Guid id)
        {
            var tickets = _context.Tickets.Where(t => t.JourneyId.Equals(id))
                .Select(t => new ReturnedTicketDto
                {
                    ArrivalTime = t.ArrivalTime,
                    LeavingTime = t.LeavingTime,
                    DestinationBusStopName = t.DestinationName,
                    JourneyId = t.JourneyId,
                    Price = t.Price,
                    SeatNumber = t.SeatNum,
                    StartBusStopName = t.StartBusStopName
                }).AsNoTracking().AsEnumerable();

            if (tickets is null)
                throw new NullReferenceException($"{id} doesn't exist");

            return Task.FromResult(tickets);
        }

        public Task<IEnumerable<ReturnedTicketDto>> GetAllTicketsByUserId(string id)
        {
            var Tickets = _context.Tickets.Where(x => x.ConsumerId == id)
                .Select(t => new ReturnedTicketDto
                {
                    ArrivalTime = t.ArrivalTime,
                    LeavingTime = t.LeavingTime,
                    DestinationBusStopName = t.DestinationName,
                    JourneyId = t.JourneyId,
                    Price = t.Price,
                    SeatNumber = t.SeatNum,
                    StartBusStopName = t.StartBusStopName
                }).AsNoTracking().AsEnumerable();
            return Task.FromResult(Tickets);
        }

        public Task<IEnumerable<ReturnedTicketDto>> GetAllBookedTickets()
        {
            var Tickets = _context.Tickets.Where(x => x.ReservedOnline)
                .Select(t => new ReturnedTicketDto
                {
                    ArrivalTime = t.ArrivalTime,
                    LeavingTime = t.LeavingTime,
                    DestinationBusStopName = t.DestinationName,
                    JourneyId = t.JourneyId,
                    Price = t.Price,
                    SeatNumber = t.SeatNum,
                    StartBusStopName = t.StartBusStopName
                }).AsNoTracking().AsEnumerable();
            return Task.FromResult(Tickets);
        }

        public Task<IEnumerable<ReturnedTicketDto>> GetAllCutTickets()
        {
            var Tickets = _context.Tickets.Where(x => !x.ReservedOnline)
                .Select(t => new ReturnedTicketDto
                {
                    ArrivalTime = t.ArrivalTime,
                    LeavingTime = t.LeavingTime,
                    DestinationBusStopName = t.DestinationName,
                    JourneyId = t.JourneyId,
                    Price = t.Price,
                    SeatNumber = t.SeatNum,
                    StartBusStopName = t.StartBusStopName
                }).AsNoTracking().AsEnumerable();
            return Task.FromResult(Tickets);
        }

        public async Task<ReturnedTicketDto> GetTicketById(Guid id)
        {
            var ticket = await _context.Tickets.FirstOrDefaultAsync(t => t.Id.Equals(id));
            if (ticket == null)
                throw new NullReferenceException("Ticket Can't be found");
            return new ReturnedTicketDto
            {
                LeavingTime = ticket.LeavingTime,
                DestinationBusStopName = ticket.DestinationName,
                ArrivalTime = ticket.ArrivalTime,
                StartBusStopName = ticket.StartBusStopName,
                JourneyId = ticket.JourneyId,
                Price = ticket.Price,
                SeatNumber = ticket.SeatNum,
            };
        }


        public Task<IEnumerable<ReturnedTicketDto>> GetTicketsByReservedTime(DateTime dateTime)
        {
            var tickets = _context.Tickets.Where(x => x.CreatedTime >= dateTime);

            var returnedTickets = tickets.Select(t => new ReturnedTicketDto
            {
                ArrivalTime = t.ArrivalTime,
                LeavingTime = t.LeavingTime,
                DestinationBusStopName = t.DestinationName,
                JourneyId = t.JourneyId,
                Price = t.Price,
                SeatNumber = t.SeatNum,
                StartBusStopName = t.StartBusStopName
            }).AsNoTracking().AsEnumerable();

            return Task.FromResult(returnedTickets);
        }

    }
}
