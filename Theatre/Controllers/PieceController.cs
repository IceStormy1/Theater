using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Theater.Abstractions.Piece;
using Theater.Contracts.Theater;

namespace Theater.Controllers
{
    [ApiController]
    public class PieceController : BaseController
    {
        private readonly IPieceService _pieceService;

        public PieceController(IPieceService pieceService)
        {
            _pieceService = pieceService;
        }

        /// <summary>
        /// Получить полную информацию о пьесе по идентификатору
        /// </summary>
        /// <param name="pieceId">Идентификатор пьесы</param>
        /// <response code="200">В случае успешной регистрации</response>
        [HttpGet("{pieceId}")]
        [ProducesResponseType(typeof(PieceModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPieceById([FromRoute] Guid pieceId)
        {
            var piecesResult = await _pieceService.GetPieceById(pieceId);

            return RenderResult(piecesResult);
        }

        /// <summary>
        /// Получить краткую информацию об актуальных пьесах
        /// </summary>
        /// <response code="200">В случае успешного запроса</response>
        /// <response code="400">В случае ошибок валидации</response>
        [HttpGet]
        [ProducesResponseType(typeof(PieceShortInformationResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPieceShortInformation()
        {
            var piecesShortInformation = await _pieceService.GetPieceShortInformation();

            return Ok(new PieceShortInformationResponse { PiecesShortInformation = piecesShortInformation });
        }
    }
}
