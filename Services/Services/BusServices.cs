using IServices.IServices;
using Model.Dto;
using Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class BusServices : IBusServices
    {
        public Task AddBus(BusDto busDto)
        {
            // Implement here bitch

            throw new NotImplementedException();
        }

        public Task<Bus> EditBus(Guid Id, BusDto busDto)
        {
            // Implement here bitch
            throw new NotImplementedException();
        }

        public Task<List<Bus>> GetAllBuses()
        {
            // Implement here bitch
            throw new NotImplementedException();
        }

        public Task<Bus> GetBusById(Guid Id)
        {
            // Implement here bitch
            throw new NotImplementedException();
        }
    }
}
