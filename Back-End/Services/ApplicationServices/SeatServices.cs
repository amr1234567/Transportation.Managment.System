using Core.Dto.UserOutput;
using Core.Models;
using Infrastructure.Context;
using Interfaces.IApplicationServices;
using Microsoft.EntityFrameworkCore;

namespace Services.ApplicationServices
{
    public class SeatServices(ApplicationDbContext context) : ISeatServices
    {
        private readonly ApplicationDbContext _context = context;

        public async Task ReserveSeat(Guid id)//done
        {
            if (id == null)
                throw new ArgumentNullException($"Seat with id doesn't exist");

            var seat = await _context.Seats.FirstOrDefaultAsync(s => s.SeatId.Equals(id));

            if (seat is null)
                throw new NullReferenceException($"Seat with id {id} doesn't Exist");

            if (!seat.IsAvailable)
                throw new Exception($"Seat with id {id} not available");

            seat.IsAvailable = false;
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<SeatDto>> GetAllSeatsInBusByBusId(Guid BusId)
        {
            var seats = await _context.Seats.Where(s => s.BusId.Equals(BusId)).ToListAsync();

            return seats.Select(s => new SeatDto
            {
                IsAvailable = s.IsAvailable,
                SeatId = s.SeatId,
                SeatNum = s.SeatNum,
            });
        }
    }
}
