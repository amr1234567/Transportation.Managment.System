using Core.Dto;
using Infrastructure.Context;
using Interfaces.IApplicationServices;

namespace Services.ApplicationServices
{
    public class SeatServices : ISeatServices
    {
        private readonly ApplicationDbContext _context;

        public SeatServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task ReserveSeat(int id)
        {
            // Implement here bitch
            //edit on seat field "IsAvailable" to false
            var record = await _context.Seats.FindAsync(id);
            record.IsAvailable = false;
            await _context.SaveChangesAsync();
        }
    }
}
