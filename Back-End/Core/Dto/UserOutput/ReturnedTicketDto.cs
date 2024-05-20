using Core.Identity;
using Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto.UserOutput
{
    public class ReturnedTicketDto
    {
        public Guid TicketId { get; set; }
        public int SeatNumber { get; set; }

        public Guid? JourneyId { get; set; }

        public DateTime ArrivalTime { get; set; }
        public DateTime LeavingTime { get; set; }

        public string DestinationBusStopName { get; set; }
        public string StartBusStopName { get; set; }

        public double Price { get; set; }
    }
}
