using Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto.ServiceInput
{
    public class JourneyDto
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public DateTime LeavingTime { get; set; }

        [Required]
        public DateTime ArrivalTime { get; set; }

        [Required]
        public string DestinationId { get; set; }

        [Required]
        public string StartNusStopId { get; set; }

        [Required]
        public Guid BusId { get; set; }
        public IEnumerable<Ticket> ReservedTickets { get; set; }

    }
}
