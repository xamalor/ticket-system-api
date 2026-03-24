using TicketSystem.Application.Interfaces;
using TicketSystem.Domain.Entities;
using TicketSystem.Domain.ValueObjects;

namespace TicketSystem.Application.UseCases
{
    public class CreateTicketUseCase
    {
        private readonly ITicketRepository _repository;
        private readonly ICacheService _cache;

        public CreateTicketUseCase(ITicketRepository repository, ICacheService cache)
        {
            _repository = repository;
            _cache = cache;
        }

        public async Task<Guid> ExecuteAsync(string title, string description, string priority)
        {
            // Convert boundary → domain
            var priorityVo = TicketPriority.FromValue(priority);

            // Crear aggregate
            var ticket = Ticket.Create(title, description, priorityVo);

            // Persistir
            await _repository.AddAsync(ticket);
            await _repository.SaveChangesAsync();

            // Invalidar (opcional en create)
            _cache.Remove($"ticket-{ticket.Id}");

            return ticket.Id;

        }
    }
}
