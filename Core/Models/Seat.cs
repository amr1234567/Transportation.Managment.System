using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Seat
    {
        [Key]
        public Guid SeatId { get; set; }
        public int SeatNum { get; set; }
        public bool IsAvailable { get; set; }

        [ForeignKey(nameof(Bus))]
        public Guid BusId { get; set; }

        public Bus? Bus { get; set; }
    }
}
