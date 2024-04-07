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
    public class TicketServices(ApplicationDbContext context, UserManager<ApplicationUser> userManager, ISeatServices seatServices) : ITicketServices
    {
        private readonly ApplicationDbContext _context = context;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly ISeatServices _seatServices = seatServices;

        private async Task GenerateTicket(TicketDto ticketDto, string ConsumerId, bool Online)
        {
            var seatNum = _context.Seats.Find(ticketDto.SeatId).SeatNum;
            var TicketPrice = _context.UpcomingJourneys.Find(ticketDto.JourneyId).TicketPrice;

            var ticket = new Ticket()
            {
                Id = Guid.NewGuid(),
                CreatedTime = ticketDto.CreatedTime,
                SeaNum = seatNum,
                TimeTableId = ticketDto.JourneyId,
                ConsumerId = ConsumerId,
                ReservedOnline = Online,
                Price = TicketPrice,
                JourneyId = ticketDto.JourneyId,
            };

            await _seatServices.ReserveSeat(ticketDto.SeatId);
            await _context.Tickets.AddAsync(ticket);

            await _context.SaveChangesAsync();
        }

        public async Task<ResponseModel<bool>> CutTicket(TicketDto ticketDto, string ConsumerId)
        {
            await GenerateTicket(ticketDto, ConsumerId, false);
            return new ResponseModel<bool>
            {
                StatusCode = 200,
                Message = "Ticket Cut Successfully"
            };
        }

        public async Task<ResponseModel<bool>> BookTicket(TicketDto ticketDto, string UserId)
        {
            await GenerateTicket(ticketDto, UserId, true);
            return new ResponseModel<bool>
            {
                StatusCode = 200,
                Message = "Ticket Booked Successfully"
            };
        }

        public async Task<List<Ticket>> GetAllTickets()
        {
            var tickets = await _context.Tickets.ToListAsync();
            return tickets;
        }


        public async Task<List<Ticket>> GetAllTicketsByJourneyId(Guid id)
        {
            var tickets = await _context.Tickets.Where(t => t.JourneyId.Equals(id)).ToListAsync();

            if (tickets is null)
                throw new NullReferenceException($"{id} doesn't exist");

            return tickets;
        }

        public async Task<List<Ticket>> GetAllTicketsByUserId(string id)
        {
            var Tickets = await _context.Tickets.Where(x => x.ConsumerId == id).ToListAsync();
            return Tickets;
        }

        public async Task<List<Ticket>> GetAllBookedTickets()
        {
            var Tickets = await _context.Tickets.Where(x => x.ReservedOnline).ToListAsync();
            return Tickets;
        }

        public async Task<List<Ticket>> GetAllCutTickets()
        {
            var Tickets = await _context.Tickets.Where(x => !x.ReservedOnline).ToListAsync();
            return Tickets;
        }

        public async Task<Ticket> GetTicketById(Guid id)
        {
            return await _context.Tickets.FirstOrDefaultAsync(t => t.Id.Equals(id));
        }


        public async Task<List<Ticket>> GetTicketsByReservedTime(DateTime dateTime)
        {
            return await _context.Tickets.Where(x => x.CreatedTime >= dateTime).ToListAsync();
        }


    }
}
