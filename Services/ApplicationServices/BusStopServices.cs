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
        public async Task<List<BusStop>> GetAllBusStops() //done
        {
            //get all busStops from Db and return it
            return await _context.BusStops.ToListAsync();
        }

        public async Task<BusStop> GetBusStopById(Guid id) //done
        {
            // Implement here bitch
            //Get All Buses in busStop with id "Id" and return it
            return await _context.BusStops.FindAsync(id);
        }

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

        public Task<List<Bus>> GetBusesByBusStopId(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
