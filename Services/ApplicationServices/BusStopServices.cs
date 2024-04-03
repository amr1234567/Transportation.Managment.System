using Core.Dto;
using Core.Models;
using Infrastructure.Context;
using Interfaces.IApplicationServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ApplicationServices
{
    public class BusStopServices : IBusStopServices
    {
        private readonly ApplicationDbContext _context;

        public BusStopServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<BusStop>> GetAllBusStops() =>
            await _context.BusStops.ToListAsync();


        public async Task<BusStop> GetBusStopById(Guid id) =>
            await _context.BusStops.FindAsync(id);


        public async Task<BusStop> AddBusStop(BusStopDto busStopDto)
        {
            var buses = new List<Bus>();
            var busStop = new BusStop()
            {
                buses = buses,
                Id = Guid.NewGuid(),
                Name = busStopDto.Name
            };
            await _context.BusStops.AddAsync(busStop);
            _context.SaveChanges();
            return busStop;
        }

        public async Task<List<Bus>> GetBusesByBusStopId(Guid id)
        {
            var busStop = await _context.BusStops.Include(bs => bs.buses)
                .FirstOrDefaultAsync(bs => bs.Id.Equals(id));
            if (busStop == null)
                throw new ArgumentNullException(nameof(busStop));
            if (busStop.buses is null)
                throw new NullReferenceException(nameof(busStop.buses));

            return busStop.buses.ToList();
        }

    }
}
