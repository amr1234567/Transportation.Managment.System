using Core.Dto.UserInput;
using Core.Dto.UserOutput;
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
        Task<IEnumerable<ReturnedTicketDto>> GetAllTickets();
        Task<IEnumerable<ReturnedTicketDto>> GetAllTicketsByJourneyId(Guid id);
        Task<IEnumerable<ReturnedTicketDto>> GetAllTicketsByUserId(string id);
        Task<ReturnedTicketDto> GetTicketById(Guid id);
        Task<IEnumerable<ReturnedTicketDto>> GetTicketsByReservedTime(DateTime dateTime);
        Task<ResponseModel<bool>> CutTicket(TicketDto ticketDto, string ConsumerId);
        Task<ResponseModel<bool>> BookTicket(TicketDto ticketDto, string ConsumerId);
        Task<IEnumerable<ReturnedTicketDto>> GetAllCutTickets();
        Task<IEnumerable<ReturnedTicketDto>> GetAllBookedTickets();
    }
}
