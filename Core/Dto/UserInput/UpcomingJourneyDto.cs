using Core.Identity;
using Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto
{
    public class UpcomingJourneyDto
    {
        [Required]
        [Range(10, 100)]
        public double TicketPrice { get; set; }

        [Required]
        public string DestinationId { get; set; }

        [Required]
        public string StartBusStopId { get; set; }

        public DateTime LeavingTime { get; set; }
        public DateTime ArrivalTime { get; set; }

        [Required]
        public Guid BusId { get; set; }
    }
}
