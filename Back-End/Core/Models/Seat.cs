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

        [Required]
        [Range(1, 40)]
        public int SeatNum { get; set; }
        public bool IsAvailable { get; set; } = true;

        [ForeignKey(nameof(Bus))]
        public Guid BusId { get; set; }

        public Bus? Bus { get; set; }
    }
}
