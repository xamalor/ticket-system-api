
namespace TicketSystem.Application.DTOs
{
    public record CreateTicketRequest(
    
        string Title,
        string Description ,
        string Priority  
    );
}
