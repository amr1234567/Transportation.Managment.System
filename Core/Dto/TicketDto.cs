using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto
{
    public class TicketDto
    {
        public Seat SeatNumber { get; set; }
        public Bus Bus { get; set; }
        public Journey Journey { get; set; }
        public bool IsFinshed { get; set; }
        public DateTime ReservedTime { get; set; }
    }
}
