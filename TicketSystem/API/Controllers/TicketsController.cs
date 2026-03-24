using Microsoft.AspNetCore.Mvc;
using TicketSystem.Application.DTOs;
using TicketSystem.Application.Interfaces;
using TicketSystem.Application.UseCases;

namespace TicketSystem.API.Controllers
{


    [ApiController]
    [Route("api/[controller]")]
    public class TicketsController : ControllerBase
    {
        private readonly ITicketRepository _repository;
        private readonly CreateTicketUseCase _createTicket;
        private readonly ReopenTicketUseCase _reopenTicket;

        public TicketsController(
            ITicketRepository repository, CreateTicketUseCase createTicket, ReopenTicketUseCase reopenTicket)
        {
            _repository = repository;
            _createTicket = createTicket;
            _reopenTicket = reopenTicket;

        }


        // =======================
        // CREATE TICKET 
        // =======================
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTicketRequest request, [FromServices] CreateTicketUseCase useCase)
        {
            var id = await _createTicket.ExecuteAsync(
            request.Title,
            request.Description,
            request.Priority);

            return CreatedAtAction(nameof(GetById), new { id }, new { id });
        }

        // ===========================
        // GET TICKET
        // ===========================
        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetById(Guid id)
        //{
        //    var ticket = await _repository.GetByIdAsync(id);

        //    if (ticket == null)
        //        return NotFound();

        //    var response = new TicketResponse(
        //        ticket.Id,
        //        ticket.Title,
        //        ticket.Description,
        //        ticket.Status.Value,
        //        ticket.Priority.Value,
        //        ticket.AssignedTo,
        //        ticket.ReopenCount
        //        );

        //    return Ok(response);
        //}

        // ========================
        // REOPEN TICKET
        // ========================
        [HttpPost("{id}/reopen")]
        public async Task<IActionResult> Reopen(Guid id)
        {
            await _reopenTicket.ExecuteAsync(id);

            return Ok();
        }

        // =======================
        // RESPONSE CACHING
        // =======================
        [HttpGet("{id}")]
        //la respuesta se cachea 60 segundos
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
        public async Task<IActionResult> GetById(Guid id, [FromServices] GetTicketByIdUseCase useCase)
        {
            var result = await useCase.ExecuteAsync(id);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

    }
}
