using TicketSystem.Domain.Entities;

namespace TicketSystem.Application.Interfaces
{
    public interface ITicketRepository
    {
        Task<Ticket?> GetByIdAsync(Guid id);
        Task AddAsync(Ticket ticket);
        Task SaveChangesAsync();
        Task UpdateAsync(Ticket ticket);

    }
}
