using Core.Dto;
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


            var seatNum = _context.Seats.Find(ticketDto.SeatId).SeatNum;

            if (seatNum == null)
                throw new ArgumentNullException("Seat Doesn't Exist");

            var journey = await _context.UpcomingJourneys.Include(j => j.Destination)
                                            .Include(j => j.StartBusStop)
                                            .FirstOrDefaultAsync(j => j.Id.Equals(ticketDto.JourneyId));
            if (journey == null)
                throw new NullReferenceException("journey can't be Found");


            var ticket = new Ticket()
            {
                Id = Guid.NewGuid(),
                CreatedTime = ticketDto.CreatedTime,
                SeatNum = seatNum,
                UpcomingJourneyId = ticketDto.JourneyId,
                ConsumerId = ConsumerId,
                ReservedOnline = Online,
                Price = journey.TicketPrice,
                JourneyId = ticketDto.JourneyId,
                ArrivalTime = journey.ArrivalTime,
                DestinationId = journey.DestinationId,
                DestinationName = journey.Destination.Name,
                LeavingTime = journey.LeavingTime,
                StartBusStopId = journey.StartBusStopId,
                StartBusStopName = journey.StartBusStop.Name
            };

            await _seatServices.ReserveSeat(ticketDto.SeatId);
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

        public async Task<List<ReturnedTicketDto>> GetAllTickets()
        {
            var tickets = await _context.Tickets.Select(t => new ReturnedTicketDto
            {
                ArrivalTime = t.ArrivalTime,
                LeavingTime = t.LeavingTime,
                DestinationBusStopName = t.DestinationName,
                JourneyId = t.JourneyId,
                Price = t.Price,
                SeatNumber = t.SeatNum,
                StartBusStopName = t.StartBusStopName
            }).ToListAsync();
            if (tickets == null)
                throw new ArgumentNullException("No Tickets Exist");

            return tickets;
        }


        public async Task<List<ReturnedTicketDto>> GetAllTicketsByJourneyId(Guid id)
        {
            var tickets = await _context.Tickets.Where(t => t.JourneyId.Equals(id))
                .Select(t => new ReturnedTicketDto
                {
                    ArrivalTime = t.ArrivalTime,
                    LeavingTime = t.LeavingTime,
                    DestinationBusStopName = t.DestinationName,
                    JourneyId = t.JourneyId,
                    Price = t.Price,
                    SeatNumber = t.SeatNum,
                    StartBusStopName = t.StartBusStopName
                }).ToListAsync();

            if (tickets is null)
                throw new NullReferenceException($"{id} doesn't exist");

            return tickets;
        }

        public async Task<List<ReturnedTicketDto>> GetAllTicketsByUserId(string id)
        {
            var Tickets = await _context.Tickets.Where(x => x.ConsumerId == id)
                .Select(t => new ReturnedTicketDto
                {
                    ArrivalTime = t.ArrivalTime,
                    LeavingTime = t.LeavingTime,
                    DestinationBusStopName = t.DestinationName,
                    JourneyId = t.JourneyId,
                    Price = t.Price,
                    SeatNumber = t.SeatNum,
                    StartBusStopName = t.StartBusStopName
                }).ToListAsync();
            return Tickets;
        }

        public async Task<List<ReturnedTicketDto>> GetAllBookedTickets()
        {
            var Tickets = await _context.Tickets.Where(x => x.ReservedOnline)
                .Select(t => new ReturnedTicketDto
                {
                    ArrivalTime = t.ArrivalTime,
                    LeavingTime = t.LeavingTime,
                    DestinationBusStopName = t.DestinationName,
                    JourneyId = t.JourneyId,
                    Price = t.Price,
                    SeatNumber = t.SeatNum,
                    StartBusStopName = t.StartBusStopName
                }).ToListAsync();
            return Tickets;
        }

        public async Task<List<ReturnedTicketDto>> GetAllCutTickets()
        {
            var Tickets = await _context.Tickets.Where(x => !x.ReservedOnline)
                .Select(t => new ReturnedTicketDto
                {
                    ArrivalTime = t.ArrivalTime,
                    LeavingTime = t.LeavingTime,
                    DestinationBusStopName = t.DestinationName,
                    JourneyId = t.JourneyId,
                    Price = t.Price,
                    SeatNumber = t.SeatNum,
                    StartBusStopName = t.StartBusStopName
                }).ToListAsync();
            return Tickets;
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


        public async Task<List<ReturnedTicketDto>> GetTicketsByReservedTime(DateTime dateTime)
        {
            return await _context.Tickets.Where(x => x.CreatedTime >= dateTime)
                .Select(t => new ReturnedTicketDto
                {
                    ArrivalTime = t.ArrivalTime,
                    LeavingTime = t.LeavingTime,
                    DestinationBusStopName = t.DestinationName,
                    JourneyId = t.JourneyId,
                    Price = t.Price,
                    SeatNumber = t.SeatNum,
                    StartBusStopName = t.StartBusStopName
                }).ToListAsync();
        }

    }
}
