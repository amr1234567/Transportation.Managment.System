using Core.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class BusStop
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 10)]
        public string Name { get; set; }

        public List<Bus>? buses { get; set; } = new List<Bus>();
        [Required]
        [ForeignKey(nameof(manger))]
        public string? managerId { get; set; }
        public BusStopManger? manger { get; set; }
    }
}
