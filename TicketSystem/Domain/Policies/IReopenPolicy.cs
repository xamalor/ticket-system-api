using TicketSystem.Domain.Entities;
using TicketSystem.Domain.ValueObjects;

namespace TicketSystem.Domain.Policies
{
    public interface IReopenPolicy
    {
       ReopenLimit GetLimit(Ticket ticket, DateTime currentDate);
    }
}
