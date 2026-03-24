
namespace TicketSystem.Domain.Events
{
    public class TicketReopenedEvent : IDomainEvent
    {
        /*ESTE EVENTO SIGNIFICA = El ticket fue reabierto*/
        public Guid TicketId { get; }
        public DateTime OccurredOn { get; }

        public TicketReopenedEvent(Guid ticketId)
        {
            TicketId = ticketId;
            OccurredOn = DateTime.UtcNow;
        }
    }
}
