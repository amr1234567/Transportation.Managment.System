using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto.UserOutput
{
    public class SeatDto
    {
        public Guid SeatId { get; set; }
        public int SeatNum { get; set; }
        public bool IsAvailable { get; set; }
    }
}
