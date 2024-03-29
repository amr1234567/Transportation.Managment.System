using Model.Dto;
using Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IServices.IServices
{
    public interface ITicketServices
    {
        Task<List<Ticket>> GetAllTicket();
        Task<List<Ticket>> GetAllTicketsByJourneyId(Guid id);
        Task<List<Ticket>> GetAllTicketsByUserId(Guid id);
        Task<Ticket> GetTicketById(Guid id);
        Task<List<Ticket>> GetTicketsByReservedTime(DateTime dateTime);
        Task GenerateTicket(TicketDto ticketDto);
        Task<Ticket> DestroyTicket(TicketDto ticketDto);
    }
}
