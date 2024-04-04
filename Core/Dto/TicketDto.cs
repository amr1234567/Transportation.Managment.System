using System.ComponentModel.DataAnnotations;

namespace Core.Dto
{
    public class TicketDto
    {
        [Required]
        public DateTime CreatedTime { get; set; }
        [Required]
        public Guid SeatId { get; set; }
        [Required]
        public Guid JourneyId { get; set; }
    }
}