
using TicketSystem.Domain.Entities;
using TicketSystem.Domain.ValueObjects;

namespace TicketSystem.Domain.Policies
{
    public class DecemberUnlimitedReopenPolicy : IReopenPolicy
    {
        public ReopenLimit GetLimit(Ticket ticket, DateTime currentDate)
        {
            throw new NotImplementedException();
        }

        public int GetMaxReopenAllowed(Ticket ticket, DateTime currentDate)
        {
            if (currentDate.Month == 12)
                return int.MaxValue;

            return 2;
        }
    }
}
