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

        public async Task AddBus(BusDto busDto)
        {
            var Busid = Guid.NewGuid();

            List<Seat> seats = new List<Seat>();
            for (int i = 0; i < busDto.NumberOfSeats; i++)
            {
                seats.Add(new Seat()
                {
                    SeatNum = i,
                    IsAvailable = true,
                    SeatId = Guid.NewGuid(),
                    BusId = Busid
                });
            }

            Bus bus = new Bus()
            {
                Id = Busid,
                seats = seats,
                NumberOfSeats = busDto.NumberOfSeats
            };
            await _context.Buses.AddAsync(bus);
            await _context.SaveChangesAsync();
        }

        public Task<Bus> EditBus(Guid Id, BusDto busDto)
        {
            // Implement here bitch
            //edit the bus data with Id with busDto Data 

            throw new NotImplementedException();
        }

        public Task<List<Bus>> GetAllBuses()
        {
            // Implement here bitch
            //get all the buses in DataBase and return it

            throw new NotImplementedException();
        }

        public async Task<Bus> GetBusById(Guid Id)
        {
            return await _context.Buses.FirstOrDefaultAsync(x => x.Id.Equals(Id));
        }
    }
}
