using Core.Dto.UserOutput;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Helpers.Functions
{
    public static class TicketsConvertor
    {
        public static List<ReturnedTicketDto> Convert(this List<Ticket> tickets)
        {
            var returningTickets = new List<ReturnedTicketDto>();
            foreach (var ticket in tickets)
            {
                returningTickets.Add(new ReturnedTicketDto
                {
                    CreatedTime = ticket.CreatedTime,
                    JourneyId = ticket.JourneyId,
                    Price = ticket.Price,
                    SeatNumber = ticket.SeaNum,
                    ConsumerId = ticket.ConsumerId
                });
            }
            return returningTickets;
        }
    }
}
