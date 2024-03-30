using Core.Dto;
using Core.Models;
using Infrastructure.Context;
using Interfaces.IApplicationServices;

namespace Services.ApplicationServices
{
    public class BusServices : IBusServices
    {
        private readonly ApplicationDbContext _context;

        public BusServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task AddBus(BusDto busDto)
        {
            // Implement here bitch
            //add new bus to database

            throw new NotImplementedException();
        }

        public Task<Bus> EditBus(Guid Id, BusDto busDto)
        {
            // Implement here bitch
            //edit the bus data with Id with busDto Data 

            throw new NotImplementedException();
        }

        public Task<List<Bus>> GetAllBuses()
        {
            // Implement here bitch
            //get all the buses in DataBase and return it

            throw new NotImplementedException();
        }

        public Task<Bus> GetBusById(Guid Id)
        {
            // Implement here bitch
            //Get Bus With the id "Id"

            throw new NotImplementedException();
        }
    }
}
