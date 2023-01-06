using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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
        /// Получить краткую информацию об актуальных пьесах
        /// </summary>
        /// <response code="200">В случае успешной регистрации</response>
        /// <response code="400">В случае ошибок валидации</response>
        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyCollection<PieceShortInformationModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPieceShortInformation()
        {
            var piecesResult = await _pieceService.GetPieceShortInformation();

            return Ok(piecesResult);
        }
    }
}
