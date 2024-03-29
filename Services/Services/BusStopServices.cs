using IServices.IServices;
using Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class BusStopServices : IBusStopServices
    {
        public Task<List<BusStop>> busStops()
        {
            // Implement here bitch
            throw new NotImplementedException();
        }

        public Task<List<Bus>> GetBusesByBusStopId(Guid id)
        {
            // Implement here bitch
            throw new NotImplementedException();
        }
    }
}
