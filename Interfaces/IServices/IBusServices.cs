using Model.Dto;
using Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IServices.IServices
{
    public interface IBusServices
    {
        Task AddBus(BusDto busDto);
        Task<List<Bus>> GetAllBuses();
        Task<Bus> GetBusById(Guid Id);
        Task<Bus> EditBus(Guid Id, BusDto busDto);
    }
}
