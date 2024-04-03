using Core.Dto;
using Core.Models;
using Infrastructure.Context;
using Interfaces.IApplicationServices;
using Microsoft.EntityFrameworkCore;

namespace Services.ApplicationServices
{
    public class BusStopServices(ApplicationDbContext context) : IBusStopServices
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<List<BusStop>> GetAllBusStops() =>
            await _context.BusStops.Include(bs=>bs.manger).Include(bs => bs.buses).ToListAsync();


        public async Task<BusStop> GetBusStopById(Guid id) =>
            await _context.BusStops.FindAsync(id);


        public async Task<BusStop> AddBusStop(BusStopDto busStopDto, string ManagerId)
        {
            var busStop = new BusStop()
            {
                Id = Guid.NewGuid(),
                Name = busStopDto.Name,
                managerId = ManagerId
            };

            await _context.BusStops.AddAsync(busStop);
            await _context.SaveChangesAsync();

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
