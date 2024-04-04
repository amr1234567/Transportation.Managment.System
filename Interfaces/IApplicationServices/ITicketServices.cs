using Core.Dto;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.IApplicationServices
{
    public interface ITicketServices
    {
        Task<List<Ticket>> GetAllTickets();
        Task<List<Ticket>> GetAllTicketsByJourneyId(Guid id);
        Task<List<Ticket>> GetAllTicketsByUserId(Guid id);
        Task<Ticket> GetTicketById(Guid id);
        Task<List<Ticket>> GetTicketsByReservedTime(DateTime dateTime);
        Task<ResponseModel<bool>> BookTicket(TicketDto ticketDto, string UserId);
        Task<ResponseModel<bool>> CutTicket(TicketDto ticketDto);
    }
}
