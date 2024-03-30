using Core.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Ticket
    {
        public Guid Id { get; set; }
        public double Price { get; set; }
        public Seat Seat { get; set; }

        [ForeignKey(nameof(Bus))]
        public Guid BusId { get; set; }
        public Bus Bus { get; set; }

        [ForeignKey(nameof(Journey))]
        public Guid JourneyId { get; set; }
        public Journey Journey { get; set; }

        public bool IsFinished => Journey.IsEnded;
        public DateTime CreatedTime { get; set; }

        public Guid UserId { get; set; }
    }
}
