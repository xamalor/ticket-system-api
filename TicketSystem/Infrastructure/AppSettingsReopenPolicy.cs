using TicketSystem.Domain.Entities;
using TicketSystem.Domain.Policies;
using TicketSystem.Domain.ValueObjects;

namespace TicketSystem.Infrastructure
{
    public class AppSettingsReopenPolicy : IReopenPolicy
    {
        private readonly IConfiguration _config;

        public AppSettingsReopenPolicy(IConfiguration config)
        {
            _config = config;
        }

        public ReopenLimit GetLimit(Ticket ticket, DateTime currentDate)
        {
            throw new NotImplementedException();
        }

        public int GetMaxReopenAllowed(Ticket ticket, DateTime currentDate)
        {
            return _config.GetValue<int>("TicketSettings:MaxReopenCount");
        }
    }
}
