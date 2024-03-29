using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Ticket
    {
        public Guid Id { get; set; }
        public string Price { get; set; }
        public Seat SeatNumber { get; set; }
        public Bus Bus { get; set; }
        public Journey Journey { get; set; }
        public bool IsFinshed { get; set; }
        public DateTime ReservedTime { get; set; }

    }
}
