using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicketSystem.Domain.Entities;

namespace TicketSystem.Infrastructure.Persistence.Configurations
{
    public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
    {
        public void Configure(EntityTypeBuilder<Ticket> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Title).IsRequired().HasMaxLength(200);

            builder.Property(t => t.Description).HasMaxLength(1000);

            builder.Property(t => t.AssignedTo).HasMaxLength(100);
            builder.Property(t => t.ReopenCount);

            builder.Property(t => t.CreatedAt);
            builder.Property(t => t.UpdatedAt);

            //  Optimistic Concurrency
            builder.Property(t => t.RowVersion).IsRowVersion();

            // Ignorar DomainEvents en EF Core
            // porque DomainEvents no deben persistirse
            builder.Ignore(t => t.DomainEvents);

        }
    }
}
