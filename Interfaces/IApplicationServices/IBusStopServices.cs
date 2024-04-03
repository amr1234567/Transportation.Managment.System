using Core.Dto;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.IApplicationServices
{
    public interface IBusStopServices
    {
        Task<List<BusStop>> GetAllBusStops();
        Task<List<Bus>> GetBusesByBusStopId(Guid id);
        Task<BusStop> AddBusStop(BusStopDto model, string ManagerId);
    }
}
