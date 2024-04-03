using Core.Dto;
using Core.Models;
using Infrastructure.Context;
using Interfaces.IApplicationServices;
using Microsoft.EntityFrameworkCore;

namespace Services.ApplicationServices
{
    public class BusServices(ApplicationDbContext context) : IBusServices
    {
        private readonly ApplicationDbContext _context = context;

        public async Task AddBus(BusDto busDto) //done
        {
            if (busDto is null)
                throw new ArgumentNullException(nameof(busDto));
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
        public async Task<ResponseModel<Bus>> EditBus(Guid Id, BusDto busDto) //done
        {
            // Implement here bitch
            //edit the bus data with Id with busDto Data 
            if (busDto is null)
                return new ResponseModel<Bus>
                {
                    StatusCode = 400,
                    Message = "Input is invalid"
                };

            var bus = await GetBusById(Id);

            if (bus is null)
                return new ResponseModel<Bus>
                {
                    StatusCode = 400,
                    Message = "id is invalid"
                };

            bus.NumberOfSeats = busDto.NumberOfSeats;

            await _context.SaveChangesAsync();

            return new ResponseModel<Bus>
            {
                StatusCode = 200,
                Message = " Done",
                Body = bus
            };
        }

        public async Task<List<Bus>> GetAllBuses() =>  //done
            await _context.Buses.Include(b => b.seats).ToListAsync();

        public async Task<Bus> GetBusById(Guid Id) =>
            await _context.Buses.FirstOrDefaultAsync(x => x.Id.Equals(Id));

    }
}