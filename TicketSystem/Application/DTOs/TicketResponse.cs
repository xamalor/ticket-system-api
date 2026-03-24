
namespace TicketSystem.Application.DTOs
{
    public record TicketResponse (
         Guid Id ,
         string Title ,
         string Description ,
         string Status ,
         string Priority ,
         string? AssignedTo ,
         int ReopenCount 
    );
}
