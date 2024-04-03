using Infrastructure.Context;
using Interfaces.IApplicationServices;
using Microsoft.EntityFrameworkCore;

namespace Services.ApplicationServices
{
    public class SeatServices : ISeatServices
    {
        private readonly ApplicationDbContext _context;

        public SeatServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task ReserveSeat(Guid id)
        {
            var seat = await _context.Seats.FirstOrDefaultAsync(s => s.SeatId.Equals(id));
            if (seat is null)
                throw new NullReferenceException(nameof(seat));
            seat.IsAvailable = false;
            await _context.SaveChangesAsync();
        }
    }
}
