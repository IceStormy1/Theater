using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Theater.Abstractions.Ticket;
using Theater.Abstractions.UserAccount.Models;
using Theater.Common;
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
        [HttpGet("piece/{ticketId:guid}/date/{dateId:guid}")]
        [ProducesResponseType(typeof(DocumentCollection<PiecesTicketModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPieceTicketsByDate([FromRoute] Guid pieceId, [FromRoute] Guid dateId)
        {
            var tickets = await Service.GetPieceTicketsByDate(pieceId, dateId);

            return Ok(new DocumentCollection<PiecesTicketModel>(tickets));
        }

        /// <summary>
        /// Получить билеты указанной пьесы по идентификатору даты пьесы 
        /// </summary>
        /// <param name="ticketId">Идентификатор пьесы</param>
        /// <response code="200">В случае успешной регистрации</response>
        [Authorize]
        [HttpGet("piece/{ticketId:guid}/buy")]
        [ProducesResponseType(typeof(WriteResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> BuyTicket([FromRoute] Guid ticketId)
        {
            if (!UserId.HasValue)
                return RenderResult(UserAccountErrors.Unauthorized);

            var buyTicketResult = await Service.BuyTicket(ticketId, UserId.Value);

            return Ok(buyTicketResult);
        }
    }
}