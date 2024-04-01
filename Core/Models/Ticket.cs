using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    public class Ticket
    {
        [Key]
        public Guid Id { get; set; }
        public Guid SeatId { get; set; }
        public Seat Seat { get; set; }

        [ForeignKey(nameof(Bus))]
        public Guid BusId { get; set; }
        public Bus Bus { get; set; }

        [ForeignKey(nameof(Journey))]
        public Guid JourneyId { get; set; }
        public Journey Journey { get; set; }

        public DateTime CreatedTime { get; set; }

        public Guid UserId { get; set; }

        public double Price => Journey.TicketPrice;
        public bool IsFinished => Journey.IsEnded;
    }
}
