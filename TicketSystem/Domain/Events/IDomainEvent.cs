namespace TicketSystem.Domain.Events
{
    public interface IDomainEvent
    {
        //Esto nos permite tipar todos los eventos del dominio.
        DateTime OccurredOn { get; }
    }
}
