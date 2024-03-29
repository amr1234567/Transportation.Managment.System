using Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dto
{
    public class BusDto
    {
        public int NumberOfSeats { get; set; }
        public List<Seat> seats { get; set; }
        public bool isFull { get; set; }
    }
}
