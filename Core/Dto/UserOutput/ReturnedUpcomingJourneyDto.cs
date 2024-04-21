using Core.Identity;
using Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto.UserOutput
{
    public class ReturnedUpcomingJourneyDto
    {
        public Guid Id { get; set; }

        public double TicketPrice { get; set; }

        public string DestinationId { get; set; }

        public string StartBusStopId { get; set; }

        public DateTime LeavingTime { get; set; }
        public DateTime ArrivalTime { get; set; }

        public Guid BusId { get; set; }

        public Guid JourneyId { get; set; }

        public int NumberOfAvailableTickets { get; set; }

        public string? DestinationName { get; set; }

        public string? StartBusStopName { get; set; }
        public IEnumerable<Seat> Seats { get; set; }
    }
}
