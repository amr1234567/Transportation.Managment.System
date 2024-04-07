using Core.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Identity;

namespace Core.Models
{
    public class UpcomingJourney
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [Range(10, 100)]
        public double TicketPrice { get; set; }

        [ForeignKey(nameof(Destination))]
        [Required]
        public string DestinationId { get; set; }
        public BusStopManger? Destination { get; set; }

        [ForeignKey(nameof(StartBusStop))]
        [Required]
        public string StartBusStopId { get; set; }
        public BusStopManger? StartBusStop { get; set; }

        public DateTime LeavingTime { get; set; }
        public DateTime ArrivalTime { get; set; }



        [ForeignKey(nameof(Bus))]
        [Required]
        public Guid BusId { get; set; }
        public Bus? Bus { get; set; }

        public List<Ticket> Ticket { get; set; }

        public Guid JourneyId { get; set; }

        public int NumberOfAvailableTickets { get; set; }



        public string? DestinationName => Destination?.Name;
        public string? StartBusStopName => StartBusStop?.Name;
        public bool IsFull => NumberOfAvailableTickets == 0;
        public bool IsLeft => LeavingTime < DateTime.UtcNow;
        public bool IsEnded => ArrivalTime < DateTime.UtcNow;
    }
}
