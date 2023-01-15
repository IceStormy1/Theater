using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Theater.Abstractions.Ticket;
using Theater.Contracts;
using Theater.Contracts.Theater;

namespace Theater.Controllers
{
    [ApiController]
    public class TicketController : BaseController<ITicketService>
    {
        public TicketController(ITicketService service) : base(service)
        {
        }

        /// <summary>
        /// Получить билеты указанной пьесы по идентификатору даты пьесы 
        /// </summary>
        /// <param name="pieceId">Идентификатор пьесы</param>
        /// <param name="dateId">Идентификатор даты пьесы</param>
        /// <response code="200">В случае успешной регистрации</response>
        [HttpGet("piece/{pieceId:guid}/date/{dateId:guid}")]
        [ProducesResponseType(typeof(DocumentCollection<PiecesTicketModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPieceTicketsByDate([FromRoute] Guid pieceId, [FromRoute] Guid dateId)
        {
            var tickets = await Service.GetPieceTicketsByDate(pieceId, dateId);

            return Ok(new DocumentCollection<PiecesTicketModel>(tickets));
        }
    }
}