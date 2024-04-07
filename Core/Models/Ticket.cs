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
        public int SeaNum { get; set; }

        public Guid? JourneyId { get; set; }

        [Required]
        public Guid? TimeTableId { get; set; }

        [Required]
        public DateTime CreatedTime { get; set; }

        [Required]
        public string? ConsumerId { get; set; }

        public bool ReservedOnline { get; set; }

        [Required]
        public double Price { get; set; }

    }
}
