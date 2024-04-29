using Core.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    public class JourneyHistory
    {
        [Key]
        public Guid Id { get; set; }


        [ForeignKey(nameof(Destination))]
        [Required]
        public string DestinationId { get; set; }
        public BusStopManger? Destination { get; set; }

        [ForeignKey(nameof(StartBusStop))]
        [Required]
        public string StartBusStopId { get; set; }
        public BusStopManger? StartBusStop { get; set; }

        public DateTime Date { get; set; }

        public DateTime LeavingTime { get; set; }
        public DateTime ArrivalTime { get; set; }

        public IEnumerable<Ticket>? Tickets { get; set; }

        [ForeignKey(nameof(Bus))]
        [Required]
        public Guid BusId { get; set; }
        public Bus? Bus { get; set; }

        public double TicketPrice { get; set; }

    }
}