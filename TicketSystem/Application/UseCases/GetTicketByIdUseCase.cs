using Microsoft.Extensions.Caching.Memory;
using TicketSystem.Application.DTOs;
using TicketSystem.Application.Interfaces;

namespace TicketSystem.Application.UseCases
{
    public class GetTicketByIdUseCase
    {
        private readonly ITicketRepository _repository;
        private readonly IMemoryCache _cache;

        public GetTicketByIdUseCase(ITicketRepository repository, IMemoryCache cache)
        { 
            _repository = repository;
            _cache = cache;
        }

        public async Task<TicketResponse?> ExecuteAsync(Guid id)
        {
            var cacheKey = $"ticket-{id}";

            var cached  = _cache.Get<TicketResponse>(cacheKey);
            if (cached != null)
                return cached;

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
