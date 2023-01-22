﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Theater.Abstractions.Piece;
using Theater.Contracts;
using Theater.Contracts.Theater;

namespace Theater.Controllers
{
    [ApiController]
    public sealed class PieceController : BaseController<IPieceService>
    {
        public PieceController(IPieceService service) : base(service)
        {
        }

        /// <summary>
        /// Получить полную информацию о пьесе по идентификатору
        /// </summary>
        /// <param name="pieceId">Идентификатор пьесы</param>
        /// <response code="200">В случае успешной регистрации</response>
        [HttpGet("{pieceId:guid}")]
        [ProducesResponseType(typeof(PieceModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPieceById([FromRoute] Guid pieceId)
        {
            var piecesResult = await Service.GetPieceById(pieceId);

            return RenderResult(piecesResult);
        }

        /// <summary>
        /// Получить краткую информацию об актуальных пьесах
        /// </summary>
        /// <response code="200">В случае успешного запроса</response>
        /// <response code="400">В случае ошибок валидации</response>
        [HttpGet]
        [ProducesResponseType(typeof(DocumentCollection<PieceShortInformationModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPiecesShortInformation()
        {
            var piecesShortInformation = await Service.GetPiecesShortInformation();

            return Ok(new DocumentCollection<PieceShortInformationModel>(piecesShortInformation));
        }
    }
}