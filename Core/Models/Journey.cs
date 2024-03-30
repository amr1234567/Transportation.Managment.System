namespace Core.Models
{
    public class Journey
    {
        public Guid Id { get; set; }

        public string? DestinationName => Destination.Name;
        public BusStop Destination { get; set; }

        public string? StartBusStopName => StartBusStop.Name;
        public BusStop StartBusStop { get; set; }

        public DateTime LeavingTime { get; set; }
        public DateTime ArrivalTime { get; set; }

        public List<Ticket> Tickets { get; set; }
        public Bus Bus { get; set; }
        public int NumberOfAvailableTickets => Bus.NumberOfSeats;

        public bool IsFull => NumberOfAvailableTickets == 0;
        public bool IsEnded => ArrivalTime < DateTime.UtcNow;
    }
}