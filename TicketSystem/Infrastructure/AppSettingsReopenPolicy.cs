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
            // Obtener el máximo permitido desde configuración
            var maxAllowed = _config.GetValue<int>("TicketSettings:MaxReopenCount");

            // Si maxAllowed == 0 → ilimitado
            if (maxAllowed == 0)
            {
                return ReopenLimit.Unlimited();
            }

            return ReopenLimit.Of(maxAllowed);
        }

        public int GetMaxReopenAllowed(Ticket ticket, DateTime currentDate)
        {
            return _config.GetValue<int>("TicketSettings:MaxReopenCount");
        }
    }
}
