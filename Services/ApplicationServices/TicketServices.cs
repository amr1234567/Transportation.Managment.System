using Core.Dto;
using Core.Identity;
using Core.Models;
using Infrastructure.Context;
using Interfaces.IApplicationServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Services.ApplicationServices
{
    public class TicketServices(ApplicationDbContext context, UserManager<ApplicationUser> userManager) : ITicketServices
    {
        private readonly ApplicationDbContext _context = context;
        private readonly UserManager<ApplicationUser> _userManager = userManager;

        private async Task<Ticket> GenerateTicket(TicketDto ticketDto, string UserId = null)
        {
            var ticket = new Ticket()
            {
                Id = Guid.NewGuid(),
                CreatedTime = ticketDto.CreatedTime,
                SeatId = ticketDto.SeatId,
                BusId = ticketDto.BusId,
                JourneyId = ticketDto.JourneyId,
                UserId = UserId
            };

            await _context.Tickets.AddAsync(ticket);
            await _context.SaveChangesAsync();
            return ticket;
        }
        public async Task<Ticket> CutTicket(TicketDto ticketDto) =>
            await GenerateTicket(ticketDto);

        public async Task<Ticket> BookTicket(TicketDto ticketDto, string UserId) =>
            await GenerateTicket(ticketDto, UserId);

        public async Task<List<Ticket>> GetAllTicket() =>
             await _context.Tickets.ToListAsync();

        public async Task<List<Ticket>> GetAllTicketsByJourneyId(Guid id)
        {
            var journey = await _context.Journeys.Include(j => j.Tickets).FirstOrDefaultAsync(j => j.Id.Equals(id));

            if (journey is null)
                throw new NullReferenceException(nameof(journey));

            return journey.Tickets;
        }

        public async Task<List<Ticket>> GetAllTicketsByUserId(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user is null)
                throw new NullReferenceException($"{nameof(user)} is null");

            return user.Tickets;
        }

        public async Task<Ticket> GetTicketById(Guid id) =>
            await _context.Tickets.FirstOrDefaultAsync(t => t.Id.Equals(id));


        public async Task<List<Ticket>> GetTicketsByReservedTime(DateTime dateTime) =>
            await _context.Tickets.Where(x => x.CreatedTime >= dateTime).ToListAsync();

    }
}
