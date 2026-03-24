using TicketSystem.Domain.Entities;
using TicketSystem.Domain.ValueObjects;

namespace TicketSystem.Domain.Policies
{
    public class HolidayReopenPolicy : IReopenPolicy
    {
        public ReopenLimit GetLimit(Ticket ticket, DateTime currentDate)
        {
            if (currentDate.Month == 12)
                return ReopenLimit.Unlimited();

            return ReopenLimit.Of(2);
        }
    }
}

