using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    public class Journey
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [Range(10, 100)]
        public double TicketPrice { get; set; }

        [ForeignKey(nameof(Destination))]
        [Required]
        public Guid DestinationId { get; set; }
        public BusStop? Destination { get; set; }

        [ForeignKey(nameof(StartBusStop))]
        [Required]
        public Guid StartBusStopId { get; set; }
        public BusStop? StartBusStop { get; set; }

        public DateTime LeavingTime { get; set; }
        public DateTime ArrivalTime { get; set; }

        public List<Ticket>? Tickets { get; set; }

        [ForeignKey(nameof(Bus))]
        [Required]
        public Guid BusId { get; set; }
        public Bus? Bus { get; set; }

        public string? DestinationName => Destination?.Name;
        public string? StartBusStopName => StartBusStop?.Name;
        public int NumberOfAvailableTickets => Bus.NumberOfSeats;
        public bool IsFull => NumberOfAvailableTickets == 0;
        public bool IsLeft => LeavingTime < DateTime.UtcNow;
        public bool IsEnded => ArrivalTime < DateTime.UtcNow;
    }
}