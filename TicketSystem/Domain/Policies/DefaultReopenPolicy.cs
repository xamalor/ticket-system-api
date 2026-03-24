
using TicketSystem.Domain.Entities;
using TicketSystem.Domain.ValueObjects;

namespace TicketSystem.Domain.Policies
{
    public class DefaultReopenPolicy : IReopenPolicy
    {
        private readonly int _limit;

        public DefaultReopenPolicy(int limit)
        {
            _limit = limit;
        }

        public ReopenLimit GetLimit(Ticket ticket, DateTime currentDate)
        {
            return ReopenLimit.Of(_limit);
        }
       
    }
}
