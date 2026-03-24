using TicketSystem.Application.Interfaces;
using TicketSystem.Domain.Policies;

namespace TicketSystem.Application.UseCases
{
    public class ReopenTicketUseCase
    {
        private readonly ITicketRepository _repository;
        private readonly IReopenPolicy _policy;
        private readonly ICacheService _cache;


        public ReopenTicketUseCase(ITicketRepository repository,IReopenPolicy policy, ICacheService cache)
        {
            _repository = repository;
            _policy = policy;
            _cache = cache;
        }

        public async Task ExecuteAsync(Guid ticketId)
        {
            //1.Obtener aggregate
            var ticket = await _repository.GetByIdAsync(ticketId);

            if (ticket == null)
                throw new Exception("Ticket not found");

            //2.Obtener regla del negocio
            var limit = _policy.GetLimit(ticket, DateTime.UtcNow);

            //3.Ejecutar comportamiento del dominio
            ticket.Reopen(limit);

            //4.Persistir cambios
            await _repository.SaveChangesAsync();

            // Invalidar Cache
            _cache.Remove($"ticket-{ticketId}");
        }
    }
}
