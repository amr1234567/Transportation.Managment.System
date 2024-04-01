using Core.Dto;
using Core.Models;
using Infrastructure.Context;
using Interfaces.IApplicationServices;
using Microsoft.EntityFrameworkCore;

namespace Services.ApplicationServices
{
    public class BusServices : IBusServices
    {
        private readonly ApplicationDbContext _context;

        public BusServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddBus(BusDto busDto) //done
        {
            var Busid = Guid.NewGuid();
            List<Seat> seats = new List<Seat>();

            Bus bus = new Bus()
            {
                Id = Busid,
                seats = seats,
                NumberOfSeats = busDto.NumberOfSeats
            };

            for (int i = 1; i <= busDto.NumberOfSeats; i++)
            {
                seats.Add(new Seat()
                {
                    SeatNum = i,
                    IsAvailable = true,
                    SeatId = Guid.NewGuid(),
                    BusId = Busid
                });
            }
            await _context.Buses.AddAsync(bus);
            await _context.SaveChangesAsync();
        }
        public async Task<Bus> EditBus(Guid Id, BusDto busDto) //done
        {
            // Implement here bitch
            //edit the bus data with Id with busDto Data 
            var bus = await _context.Buses.FindAsync(Id);
            bus.NumberOfSeats = busDto.NumberOfSeats;
            await _context.SaveChangesAsync();
            return bus;
        }

        public async Task<List<Bus>> GetAllBuses() =>  //done
            await _context.Buses.Include(b => b.seats).ToListAsync();

        public async Task<Bus> GetBusById(Guid Id) //done
        {
            return await _context.Buses.FirstOrDefaultAsync(x => x.Id.Equals(Id));
        }
    }
}