using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Theater.Abstractions.Piece;
using Theater.Contracts;
using Theater.Contracts.Theater;

namespace Theater.Controllers.Admin
{
    [Authorize]
    [Route("api/admin/piece")]
    public sealed class PieceAdminController : BaseController<IPieceService>
    {
        public PieceAdminController(IPieceService service) : base(service)
        {
        }

        /// <summary>
        /// Создать пьесу
        /// </summary>
        /// <response code="200">В случае успешного запроса</response>
        /// <response code="400">В случае ошибок валидации</response>
       // [Authorize(Policy = nameof(RoleModel.User.Policies.UserSearch))]
        [HttpPost]
        [ProducesResponseType(typeof(DocumentMeta), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreatePiece([FromBody] PieceParameters parameters)
            => await CreateOrUpdatePiece(parameters);
        
        /// <summary>
        /// Создать пьесу
        /// </summary>
        /// <response code="200">В случае успешного запроса</response>
        /// <response code="400">В случае ошибок валидации</response>
        // [Authorize(Policy = nameof(RoleModel.User.Policies.UserSearch))]
        [HttpPut("{pieceId:guid}")]
        [ProducesResponseType(typeof(DocumentMeta), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePiece([FromBody] PieceParameters parameters, [FromRoute] Guid pieceId)
            => await CreateOrUpdatePiece(parameters, pieceId);

        /// <summary>
        /// Создать пьесу
        /// </summary>
        /// <response code="200">В случае успешного запроса</response>
        /// <response code="400">В случае ошибок валидации</response>
        // [Authorize(Policy = nameof(RoleModel.User.Policies.UserSearch))]
        [HttpDelete("{pieceId:guid}")]
        [ProducesResponseType(typeof(DocumentMeta), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeletePiece([FromRoute] Guid pieceId)
        {
            var deletePieceResult = await Service.DeletePiece(pieceId);

            return RenderResult(deletePieceResult);
        }

        /// <summary>
        /// Обновить или создать пьесу
        /// </summary>
        /// <param name="parameters">Параметры</param>
        /// <param name="pieceId">Идентификатор пьесы</param>
        /// <remarks>
        /// Идентификатор <paramref name="pieceId"/> указывается при обновлении пьесы
        /// </remarks>
        private async Task<IActionResult> CreateOrUpdatePiece(PieceParameters parameters, Guid? pieceId = null)
        {
            var piecesShortInformation = await Service.CreateOrUpdatePiece(parameters, pieceId);

            return RenderResult(piecesShortInformation);
        }
    }
}