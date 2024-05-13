using Core.Identity;
using Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace Core.Dto
{
    public class UpcomingJourneyDto
    {
        [Required]
        [Range(10, 100)]
        public double TicketPrice { get; set; }

        [Required]
        public string DestinationId { get; set; }

        [JsonIgnore]
        public string StartBusStopId { get; set; } = "";

        [Required]
        public DateTime LeavingTime { get; set; }
        [Required]
        public DateTime ArrivalTime { get; set; }

        [Required]
        public string BusId { get; set; }
    }
}
