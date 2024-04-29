using Core.Dto.UserOutput;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Helpers.Functions
{
    public static class TicketConvertor
    {
        public static ReturnedTicketDto ConvertToDto(this Ticket ticket) =>
            new ReturnedTicketDto
            {
                JourneyId = ticket.JourneyId,
                Price = ticket.Price,
                SeatNumber = ticket.SeatNum,
                StartBusStopName = ticket.StartBusStopName,
                ArrivalTime = ticket.ArrivalTime,
                DestinationBusStopName = ticket.DestinationName,
                LeavingTime = ticket.LeavingTime
            };
    }
}
