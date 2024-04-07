using Core.Dto;
using Core.Dto.UserOutput;
using Core.Models;
using Infrastructure.Context;
using Interfaces.IApplicationServices;
using Microsoft.EntityFrameworkCore;
using System;

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
                NumberOfSeats = busDto.NumberOfSeats,
                IsAvailable=true
                
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
            if (busDto is null)
                return new ResponseModel<Bus>
                {
                    StatusCode = 400,
                    Message = "Input is invalid"
                };

            var bus = await _context.Buses.Include(b => b.seats)
                                            .FirstOrDefaultAsync(b => b.Id.Equals(Id));

            if (bus is null)
                return new ResponseModel<Bus>
                {
                    StatusCode = 400,
                    Message = "id is invalid"
                };

            if (bus.NumberOfSeats < busDto.NumberOfSeats)
            {
                List<Seat> seats = new List<Seat>();

                for (int i = bus.NumberOfSeats + 1; i <= busDto.NumberOfSeats; i++)
                {
                    seats.Add(new Seat()
                    {
                        SeatNum = i,
                        IsAvailable = true,
                        SeatId = Guid.NewGuid(),
                        BusId = Id
                    });
                }
                bus.seats = [.. bus.seats, .. seats];
                _context.Seats.AddRange(seats);
            }
            else if (bus.NumberOfSeats > busDto.NumberOfSeats)
            {
                var removedSeats = bus.seats.OrderBy(s => s.SeatNum)
                                            .TakeLast(bus.NumberOfSeats - busDto.NumberOfSeats)
                                            .ToList();

                foreach (var seat in removedSeats)
                    bus.seats.Remove(seat);
            }

            bus.NumberOfSeats = busDto.NumberOfSeats;

            await _context.SaveChangesAsync();

            return new ResponseModel<Bus>
            {
                StatusCode = 200,
                Message = " Done",
                Body = bus
            };
        }

        public async Task<List<Bus>> GetAllBuses() =>
            await _context.Buses.Include(b => b.seats).ToListAsync();

        public async Task<Bus> GetBusById(Guid Id)
        {
            var buses = await GetAllBuses();
            return buses.FirstOrDefault(x => x.Id.Equals(Id));
        }

    }
}