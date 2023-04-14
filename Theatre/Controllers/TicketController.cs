using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using AutoMapper;
using Theater.Abstractions.Ticket;
using Theater.Common;
using Theater.Contracts;
using Theater.Contracts.Theater;
using Theater.Entities.Theater;
using Theater.Abstractions.Errors;

namespace Theater.Controllers
{
    [ApiController]
    public sealed class TicketController : BaseController<PiecesTicketParameters, PiecesTicketEntity>
    {
        private readonly IPieceTicketService _pieceTicketService;

        public TicketController(
            IPieceTicketService service, 
            IMapper mapper) : base(service, mapper)
        {
            _pieceTicketService = service;
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
            var tickets = await _pieceTicketService.GetPieceTicketsByDate(pieceId, dateId);

            return Ok(new DocumentCollection<PiecesTicketModel>(tickets));
        }

        /// <summary>
        /// Купить билет 
        /// </summary>
        /// <param name="ticketId">Идентификатор пьесы</param>
        /// <response code="200">В случае успешной регистрации</response>
        /// <response code="400">В случае ошибок валидации</response>
        [Authorize]
        [HttpPost("piece/{ticketId:guid}/buy")]
        [ProducesResponseType(typeof(WriteResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(WriteResult), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> BuyTicket([FromRoute] Guid ticketId)
        {
            if (!UserId.HasValue)
                return RenderResult(UserAccountErrors.Unauthorized);

            var buyTicketResult = await _pieceTicketService.BuyTicket(ticketId, UserId.Value);

            return RenderResult(buyTicketResult);
        }

        /// <summary>
        /// Забронировать билет
        /// </summary>
        /// <param name="ticketId">Идентификатор пьесы</param>
        /// <response code="200">В случае успешной регистрации</response>
        /// <response code="400">В случае ошибок валидации</response>
        [Authorize]
        [HttpPost("piece/{ticketId:guid}/book")]
        [ProducesResponseType(typeof(WriteResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(WriteResult), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> BookTicket([FromRoute] Guid ticketId)
        {
            if (!UserId.HasValue)
                return RenderResult(UserAccountErrors.Unauthorized);

            var buyTicketResult = await _pieceTicketService.BookTicket(ticketId, UserId.Value);

            return RenderResult(buyTicketResult);
        }
    }
}