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
            await _context.BusStops.Include(bs => bs.manger)
                                    .Include(bs => bs.buses)
                                    .ToListAsync();


        public async Task<BusStop> GetBusStopById(Guid id)
        {
            var busStops = await GetAllBusStops();
            return busStops.FirstOrDefault(bs => bs.Id.Equals(id));
        }

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
            var busStops = await GetAllBusStops();
            var busStop = busStops.FirstOrDefault(bs => bs.Id.Equals(id));

            if (busStop == null)
                throw new ArgumentNullException($"busStop with {id} doesn't exist");

            if (busStop.buses is null)
                throw new NullReferenceException($"there are no buses in this busStop");

            return busStop.buses.ToList();
        }

    }
}
