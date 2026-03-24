using Microsoft.EntityFrameworkCore;
using TicketSystem.Domain.Entities;
using TicketSystem.Domain.ValueObjects;

namespace TicketSystem.Infrastructure.Persistence
{
    public class TicketDbContext : DbContext
    {
        public TicketDbContext(DbContextOptions<TicketDbContext> options) : base(options) { }

        public DbSet<Ticket> Tickets => Set<Ticket>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.HasKey(t => t.Id);

                entity.Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(200);

                entity.Property(t => t.Description)
                .HasMaxLength(1000);

                // 🔥 Value Converter for TicketStatus
                entity.Property(t => t.Status)
                      .HasConversion(
                          v => v.Value,
                          v => TicketStatus.FromValue(v))
                      .IsRequired();

                // 🔥 Value Converter for TicketPriority
                entity.Property(t => t.Priority)
                      .HasConversion(
                          v => v.Value,
                          v => TicketPriority.FromValue(v))
                      .IsRequired();

                // Concurrency
                entity.Property(t => t.RowVersion)
                      .IsRowVersion();

            });
        }
    }
}
