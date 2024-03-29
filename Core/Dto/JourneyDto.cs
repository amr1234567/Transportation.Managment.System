using Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dto
{
    public class JourneyDto
    {
        public string Destination { get; set; }
        public string StartBusStop { get; set; }
        public DateTime LeavingTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public List<Ticket> ReservedTickets { get; set; }
        public bool IsEnded { get; set; }
    }
}
