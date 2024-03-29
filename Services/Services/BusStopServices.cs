using Core.Models;
using Infrastructure.Context;
using Interfaces.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class BusStopServices : IBusStopServices
    {
        private readonly ApplicationDbContext _context;

        public BusStopServices(ApplicationDbContext context)
        {
            _context = context;
        }
        public Task<List<BusStop>> GetAllBusStops()
        {
            //get all busStops from Db and return it

            throw new NotImplementedException();
        }

        public Task<List<Bus>> GetBusesByBusStopId(Guid id)
        {
            // Implement here bitch
            //Get All Buses in busStop with id "Id" and return it
            throw new NotImplementedException();
        }
    }
}
