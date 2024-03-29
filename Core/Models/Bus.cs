using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Bus
    {
        public Guid Id { get; set; }
        public int NumberOfSeats { get; set; }
        public List<Seat> seats { get; set; }
        public bool isFull { get; set; }
    }
}
