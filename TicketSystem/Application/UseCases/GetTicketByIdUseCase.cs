using Microsoft.Extensions.Caching.Memory;
using TicketSystem.Application.DTOs;
using TicketSystem.Application.Interfaces;

namespace TicketSystem.Application.UseCases
{
    public class GetTicketByIdUseCase
    {
        private readonly ITicketRepository _repository;
        private readonly IMemoryCache _cache;
        private readonly ILogger<GetTicketByIdUseCase> _logger;

        public GetTicketByIdUseCase(ITicketRepository repository, IMemoryCache cache, ILogger<GetTicketByIdUseCase> logger)
        { 
            _repository = repository;
            _cache = cache;
            _logger = logger;
        }

        public async Task<TicketResponse?> ExecuteAsync(Guid id)
        {
            var cacheKey = $"ticket-{id}";

            var cached  = _cache.Get<TicketResponse>(cacheKey);
            if (cached != null)
            {
                _logger.LogInformation("Ticket retrieved from cache: {TicketId}", id);
                return cached;
            }

            _logger.LogInformation("Ticket retrieved from database: {TicketId}", id);

            var ticket = await _repository.GetByIdAsync(id);

            if (ticket == null)
                return null;

            var response =  new TicketResponse(
                ticket.Id, 
                ticket.Title,
                ticket.Description,
                ticket.Status.Value,
                ticket.Priority.Value,
                ticket.AssignedTo,
                ticket.ReopenCount
                );    
            
            _cache.Set(cacheKey, response, TimeSpan.FromMinutes(5));

            return response;
        }
    }
}
