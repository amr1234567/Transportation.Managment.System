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
    public class TicketServices : ITicketServices
    {
        public Task<Ticket> DestroyTicket(TicketDto ticketDto)
        {
            // Implement here bitch
            throw new NotImplementedException();
        }

        public Task GenerateTicket(TicketDto ticketDto)
        {
            // Implement here bitch
            throw new NotImplementedException();
        }

        public Task<List<Ticket>> GetAllTicket()
        {
            // Implement here bitch
            throw new NotImplementedException();
        }

        public Task<List<Ticket>> GetAllTicketsByJourneyId(Guid id)
        {
            // Implement here bitch
            throw new NotImplementedException();
        }

        public Task<List<Ticket>> GetAllTicketsByUserId(Guid id)
        {
            // Implement here bitch
            throw new NotImplementedException();
        }

        public Task<Ticket> GetTicketById(Guid id)
        {
            // Implement here bitch
            throw new NotImplementedException();
        }

        public Task<List<Ticket>> GetTicketsByReservedTime(DateTime dateTime)
        {
            // Implement here bitch
            throw new NotImplementedException();
        }
    }
}
