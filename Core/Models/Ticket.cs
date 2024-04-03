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
        [ForeignKey(nameof(Seat))]
        public Guid SeatId { get; set; }
        public Seat? Seat { get; set; }

        [Required]
        [ForeignKey(nameof(Bus))]
        public Guid BusId { get; set; }
        public Bus? Bus { get; set; }

        [Required]
        [ForeignKey(nameof(Journey))]
        public Guid JourneyId { get; set; }
        public Journey? Journey { get; set; }

        [Required]
        public DateTime CreatedTime { get; set; }

        [Required]
        [ForeignKey(nameof(User))]
        public string UserId { get; set; }
        public ApplicationUser? User { get; set; }

        public double Price => Journey.TicketPrice;
        public bool IsFinished => Journey.IsEnded;
    }
}
