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

        private async Task<ReturnedTicketDto> GenerateTicket(TicketDto ticketDto, string ConsumerId, bool Online)
        {
            ArgumentNullException.ThrowIfNull(ticketDto);
            ArgumentNullException.ThrowIfNull(ConsumerId);


            var seat = await _context.Seats.FindAsync(Guid.Parse(ticketDto.SeatId));

            if (seat == null)
                throw new ArgumentNullException("Seat Doesn't Exist");

            if (!seat.IsAvailable)
                throw new Exception("Seat Not Available");

            var journey = await _context.UpcomingJourneys.Include(j => j.Destination)
                                            .Include(j => j.StartBusStop)
                                            .FirstOrDefaultAsync(j => j.Id.Equals(Guid.Parse(ticketDto.JourneyId)));
            if (journey == null)
                throw new NullReferenceException("journey can't be Found");


            var ticket = new Ticket()
            {
                Id = Guid.NewGuid(),
                CreatedTime = DateTime.UtcNow,
                SeatNum = seat.SeatNum,
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
            return new ReturnedTicketDto
            {
                ArrivalTime = ticket.ArrivalTime,
                DestinationBusStopName = ticket.DestinationName,
                JourneyId = ticket.JourneyId,
                LeavingTime = ticket.LeavingTime,
                Price = ticket.Price,
                SeatNumber = ticket.SeatNum,
                StartBusStopName = ticket.StartBusStopName,
                TicketId = ticket.Id
            };
        }

        public async Task<ResponseModel<ReturnedTicketDto>> CutTicket(TicketDto ticketDto, string ConsumerId)
        {
            ArgumentNullException.ThrowIfNull(ticketDto);
            ArgumentNullException.ThrowIfNull(ConsumerId);

            var ticket = await GenerateTicket(ticketDto, ConsumerId, false);
            if (ticket == null)
                return new ResponseModel<ReturnedTicketDto>
                {
                    StatusCode = 500,
                    Message = "Something went wrong"
                };
            return new ResponseModel<ReturnedTicketDto>
            {
                StatusCode = 200,
                Message = "Ticket Cut Successfully",
                Body = ticket
            };
        }

        public async Task<ResponseModel<ReturnedTicketDto>> BookTicket(TicketDto ticketDto, string UserId)
        {

            ArgumentNullException.ThrowIfNull(ticketDto);
            ArgumentNullException.ThrowIfNull(UserId);

            var ticket = await GenerateTicket(ticketDto, UserId, true);
            if (ticket == null)
                return new ResponseModel<ReturnedTicketDto>
                {
                    StatusCode = 500,
                    Message = "Something went wrong"
                };
            return new ResponseModel<ReturnedTicketDto>
            {
                StatusCode = 200,
                Message = "Ticket Booked Successfully",
                Body = ticket
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
                StartBusStopName = t.StartBusStopName,
                TicketId = t.Id
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
                    StartBusStopName = t.StartBusStopName,
                    TicketId = t.Id
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
                    StartBusStopName = t.StartBusStopName,
                    TicketId = t.Id
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
                    StartBusStopName = t.StartBusStopName,
                    TicketId = t.Id
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
                    StartBusStopName = t.StartBusStopName,
                    TicketId = t.Id
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
                TicketId = ticket.Id
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
