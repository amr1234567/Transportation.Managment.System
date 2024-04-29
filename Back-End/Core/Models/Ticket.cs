using Core.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    public class Ticket
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public int SeatNum { get; set; }

        public Guid? JourneyId { get; set; }

        public Guid? UpcomingJourneyId { get; set; }

        [Required]
        public DateTime CreatedTime { get; set; }

        [Required]
        public DateTime ArrivalTime { get; set; }

        [Required]
        public DateTime LeavingTime { get; set; }

        [Required]
        public string? ConsumerId { get; set; }

        public bool ReservedOnline { get; set; }

        [Required]
        public string DestinationId { get; set; }
        public string? DestinationName { get; set; }

        [Required]
        public string StartBusStopId { get; set; }
        public string? StartBusStopName { get; set; }

        [Required]
        public double Price { get; set; }
    }
}
