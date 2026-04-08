using TicketSystem.Application.Interfaces;
using TicketSystem.Domain.Entities;
using TicketSystem.Domain.ValueObjects;
using Microsoft.Extensions.Logging;

namespace TicketSystem.Application.UseCases
{
    public class CreateTicketUseCase
    {
        private readonly ITicketRepository _repository;
        private readonly ICacheService _cache;
        private readonly ILogger<CreateTicketUseCase> _logger;


        public CreateTicketUseCase(ITicketRepository repository, ICacheService cache, ILogger<CreateTicketUseCase> logger )
        {
            _repository = repository;
            _cache = cache;
            _logger = logger;
        }

        public async Task<Guid> ExecuteAsync(string title, string description, string priority)
        {
            try
            {
                _logger.LogInformation("Starting ticket creation: {Title}", title);

                // Convert boundary → domain
                var priorityVo = TicketPriority.FromValue(priority);

                // Crear aggregate
                var ticket = Ticket.Create(title, description, priorityVo);

                // Persistir
                await _repository.AddAsync(ticket);
                await _repository.SaveChangesAsync();

                _logger.LogInformation("Ticket created successfully: {Id}", ticket.Id);

                // Invalidar (opcional en create)
                _cache.Remove($"ticket-{ticket.Id}");

                return ticket.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating ticket");
                throw;  //importante para que lo capture el middleware
            }

        }
    }
}
