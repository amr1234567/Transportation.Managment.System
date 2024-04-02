using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Bus
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [Range(25, 40)]
        public int NumberOfSeats { get; set; }

        [Required]
        public List<Seat> seats { get; set; }
    }
}
