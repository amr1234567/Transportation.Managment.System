using Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto
{
    public class JourneyDto
    {
        [Required]
        public DateTime LeavingTime { get; set; }

        [Required]
        public DateTime ArrivalTime { get; set; }

        [Required]
        public Guid DestinationId { get; set; }

        [Required]
        public Guid StartNusStopId { get; set; }

        [Required]
        public Guid BusId { get; set; }

        [Range(10, 100)]
        public double Price { get; set; }
    }
}
