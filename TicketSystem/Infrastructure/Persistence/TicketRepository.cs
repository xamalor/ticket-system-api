using Microsoft.EntityFrameworkCore;
using TicketSystem.Application.Interfaces;
using TicketSystem.Domain.Entities;

namespace TicketSystem.Infrastructure.Persistence
{
    public class TicketRepository : ITicketRepository
    {
        private readonly TicketDbContext _context;

        public TicketRepository(TicketDbContext context)
        {
            _context = context;
        }

        public async Task<Ticket?> GetByIdAsync(Guid id)
        {
            return await _context.Tickets.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task AddAsync(Ticket ticket)
        {
            await _context.Tickets.AddAsync(ticket);
        }        

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }


        //EF Core hace tracking automaticamente por eso no necesitamos Update()
        public Task UpdateAsync(Ticket ticket)
        {
            throw new NotImplementedException();
        }
    }
}
