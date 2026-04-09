using Microsoft.EntityFrameworkCore;
using TicketSystem.Application.Interfaces;
using TicketSystem.Application.UseCases;
using TicketSystem.Domain.Entities;

namespace TicketSystem.Infrastructure.Persistence
{
    public class TicketRepository : ITicketRepository
    {
        private readonly TicketDbContext _context;
        private readonly ILogger<TicketRepository> _logger;

        public TicketRepository(TicketDbContext context, ILogger<TicketRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Ticket?> GetByIdAsync(Guid id)
        {
            
                _logger.LogInformation("Searching ticket with Id: {TicketId}", id);

            try
            {
                var ticket = await _context.Tickets.FirstOrDefaultAsync(t => t.Id == id);

                if (ticket == null)
                {
                    _logger.LogWarning("Ticket with Id {TicketId} not found", id);
                }
                else
                {
                    _logger.LogInformation("Ticket with Id {TicketId} found successfully", id);
                }

                return ticket;
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error retrieving ticket with Id: {TicketId}", id);
                Console.WriteLine($"Error in GetByIdAsync: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");

                throw; // MUY IMPORTANTE: no ocultar el error
             }
               
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
