using Core.Dto;
using Core.Models;
using Interfaces.IServices;
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
            //edit on ticket field "IsFinshed" to true
            throw new NotImplementedException();
        }

        public Task GenerateTicket(TicketDto ticketDto)
        {
            // Implement here bitch
            //create new ticket and add it to DB
            throw new NotImplementedException();
        }

        public Task<List<Ticket>> GetAllTicket()
        {
            // Implement here bitch
            //get all tickets from DB and return it
            throw new NotImplementedException();
        }

        public Task<List<Ticket>> GetAllTicketsByJourneyId(Guid id)
        {
            // Implement here bitch
            //get all tickets that cutted in journey with id "id" from DB and return it

            throw new NotImplementedException();
        }

        public Task<List<Ticket>> GetAllTicketsByUserId(Guid id)
        {
            // Implement here bitch
            //get all tickets that cutted with user who has id "id" from DB and return it

            throw new NotImplementedException();
        }

        public Task<Ticket> GetTicketById(Guid id)
        {
            // Implement here bitch
            // get ticket with id "id" and return it
            throw new NotImplementedException();
        }

        public Task<List<Ticket>> GetTicketsByReservedTime(DateTime dateTime)
        {
            // Implement here bitch
            //get all tickets from "dateTime" to the present time
            throw new NotImplementedException();
        }
    }
}
