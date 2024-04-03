namespace Core.Dto
{
    public class TicketDto
    {
        public bool IsFinished { get; set; }
        public DateTime CreatedTime { get; set; }

        public Guid SeatId { get; set; }
        public Guid JourneyId { get; set; }
        public Guid BusId { get; set; }
    }
}