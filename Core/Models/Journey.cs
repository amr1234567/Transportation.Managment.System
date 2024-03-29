using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Journey
    {
        public Guid Id { get; set; }
        public string Destination { get; set; }
        public string StartBusStop { get; set; }
        public DateTime LeavingTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public List<Ticket> ReservedTickets { get; set; }
        public bool IsEnded { get; set; }
    }
}
