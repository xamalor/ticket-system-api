using Microsoft.Extensions.Diagnostics.HealthChecks;
using TicketSystem.Domain.Events;
using TicketSystem.Domain.Exceptions;
using TicketSystem.Domain.ValueObjects;

namespace TicketSystem.Domain.Entities
{
    public class Ticket
    {
        // EF Core necesita constructor vacío
        private Ticket() { } 

        //=================================
        // ENTITY IDENTITY
        //=================================
        //Identidad del Aggregate Root
        public Guid Id { get; private set; }

        //=================================
        // STATE (Aggregate state)
        //=================================
        public string Title { get; private set; }
        public string Description { get; private set;  }

        public TicketStatus Status { get; private set; }
        public TicketPriority Priority { get; private set; }

        public string? AssignedTo {  get; private set; }

        public int ReopenCount { get; private set; }

        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }


        // ==================================
        // OPTIMISTIC CONCURRENCY
        // ==================================
        // EF Core usuara este campo como RowVersion
        // para detectar conflictos cuando dos procesos
        // intentan modificar el mismo ticket al mismo tiempo
        public byte[] RowVersion { get; private set; }

        // ===================================
        // DOMAIN EVENTS SUPPORT
        // ===================================
        // esto permite que el Aggregate registre eventos internamente
        private readonly List<IDomainEvent> _domainEvents = new();
        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();


        // ===================================
        // FACTORY METHOD
        // ===================================
        // Se usa en lugar de constructor publico
        // para asegurar invariantes de creacion
        public static Ticket Create(string title, string description, TicketPriority priority)
        {
            //INVARIANT #1
            //Un ticket siempre debe tener titulo
            if(string.IsNullOrWhiteSpace(title)) 
                throw new DomainException("Title cannot be empty.");

            var ticket = new Ticket
            { 
                Id = Guid.NewGuid(),
                Title = title,
                Description = description,  
                Priority = priority,

                //INVARIANT #2
                //Todo ticket nuevo inicia en estado Open
                Status = TicketStatus.Open(),

                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                ReopenCount = 0
            };


            //DOMAIN EVENT (ejemplo)
            //ticket.AddDomainEvent(new TicketCreatedEvent(ticket.Id));

            return ticket;
        }

        public void StartProgress()
        {
            // INVARIANT #3
            // Solo tickets abiertos pueden pasar a InProgress
            if (Status != TicketStatus.Open())
                throw new DomainException("Only open tickets can move to InProgress.");

            Status = TicketStatus.InProgress();
            UpdatedAt = DateTime.UtcNow;

            // DOMAIN EVENT
            // AddDomainEvent(new TicketStartedEvent(Id));
        }

        public void Close()
        {
            // INVARIANT #4
            // Solo tickets en progreso pueden cerrarse
            if (Status != TicketStatus.InProgress())
                throw new DomainException("Only tickets in progress can be closed.");

            Status = TicketStatus.Closed();
            UpdatedAt = DateTime.UtcNow;

            // DOMAIN EVENT
            // AddDomainEvent(new TicketClosedEvent(Id));
        }

        public void Reopen(ReopenLimit limit)
        {
            // INVARIANT #5
            // Solo tickets cerrados pueden reabrirse
            if (Status != TicketStatus.Closed())
                throw new DomainException("Only closed tickets can be reopened.");

            // INVARIANT #6
            // No exceder el límite definido por la policy
            if (!limit.IsUnlimited && ReopenCount >= limit.Value)
                throw new DomainException("Reopen limit reached");

            Status = TicketStatus.Open();
            ReopenCount++;
            UpdatedAt = DateTime.UtcNow;

            // ==============================
            // DOMAIN EVENT
            // ==============================
            AddDomainEvent(new TicketReopenedEvent(Id));
        }

        public void ChangePriority(TicketPriority newPriority)
        {
            // INVARIANT #7
            // No se puede cambiar prioridad si ya está cerrado
            if (Status == TicketStatus.Closed())
                throw new DomainException("Cannot change priority of a closed ticket.");

            Priority = newPriority;
            UpdatedAt = DateTime.UtcNow;

            // DOMAIN EVENT
            // AddDomainEvent(new TicketPriorityChangedEvent(Id, newPriority));
        }

        //Metodo para agregar eventos
        private void AddDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        //Metodo para limpiar eventos
        //Esto lo usa el Repository despues de procesarlos.
        public void ClearDomainEvents()
        {
            _domainEvents.Clear(); 
        }

    }
}
