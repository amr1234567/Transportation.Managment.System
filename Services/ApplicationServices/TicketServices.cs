using Core.Dto;
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

        private async Task GenerateTicket(TicketDto ticketDto, string UserId = null)
        {
            var ticket = new Ticket()
            {
                Id = Guid.NewGuid(),
                CreatedTime = ticketDto.CreatedTime,
                SeatId = ticketDto.SeatId,
                JourneyId = ticketDto.JourneyId,
                UserId = UserId
            };

            await _seatServices.ReserveSeat(ticketDto.SeatId);
            await _context.Tickets.AddAsync(ticket);
            await _context.SaveChangesAsync();
        }

        public async Task<ResponseModel<bool>> CutTicket(TicketDto ticketDto)
        {
            await GenerateTicket(ticketDto);
            return new ResponseModel<bool>
            {
                StatusCode = 200,
                Message = "Ticket Cut Successfully"
            };
        }


        public async Task<ResponseModel<bool>> BookTicket(TicketDto ticketDto, string UserId)
        {
            await GenerateTicket(ticketDto, UserId);
            return new ResponseModel<bool>
            {
                StatusCode = 200,
                Message = "Ticket Booked Successfully"
            };
        }


        public async Task<List<Ticket>> GetAllTickets()
        {
            var ticketsWithUser = await _context.Tickets
                        .Include(t => t.Journey)
                            .ThenInclude(j => j.Bus)
                        .Include(t => t.Seat)
                        .Include(t => t.User)
                        .ToListAsync();

            var ticketsWithoutUser = await _context.Tickets
                        .Include(t => t.Journey)
                            .ThenInclude(j => j.Bus)
                        .Include(t => t.Seat)
                        .ToListAsync();

            return ticketsWithoutUser.UnionBy(ticketsWithUser, t => t.User).ToList();
        }


        public async Task<List<Ticket>> GetAllTicketsByJourneyId(Guid id)
        {
            var journeys = await _context.Journeys
                .Include(j => j.Tickets)
                    .ThenInclude(t => t.Journey)
                        .ThenInclude(j => j.Bus)
                .Include(j => j.Tickets)
                    .ThenInclude(t => t.Seat).ToListAsync();
            var journey = journeys.FirstOrDefault(j => j.Id.Equals(id));

            if (journey is null)
                throw new NullReferenceException($"{id} doesn't exist");

            return journey.Tickets;
        }

        public async Task<List<Ticket>> GetAllTicketsByUserId(Guid id)
        {
            var users = await _userManager.Users
                            .Include(u => u.Tickets)
                                .ThenInclude(t => t.Journey)
                                    .ThenInclude(j => j.Bus)
                            .Include(u => u.Tickets)
                                .ThenInclude(t => t.Seat)
                            .ToListAsync();

            var user = users.Find(u => u.Id.CompareTo(id.ToString()) == 0);

            if (user is null)
                throw new NullReferenceException($"{nameof(user)} is null");

            return user.Tickets;
        }

        public async Task<Ticket> GetTicketById(Guid id)
        {
            var tickets = await GetAllTickets();
            return tickets.FirstOrDefault(t => t.Id.Equals(id));
        }


        public async Task<List<Ticket>> GetTicketsByReservedTime(DateTime dateTime)
        {
            var tickets = await GetAllTickets();
            return tickets.Where(x => x.CreatedTime >= dateTime).ToList();
        }

    }
}
