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
        public async Task<IEnumerable<Seat>> GetAllSeatsInBusByBusId(Guid BusId)
        {
            var bus = await _context.Buses.FindAsync(BusId);
            if (bus is null)
                throw new NullReferenceException($"bus with id '{BusId}' not found");
            return bus.seats.AsEnumerable();
        }
    }
}
