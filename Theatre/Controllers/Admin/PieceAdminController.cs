using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Theater.Abstractions.Piece;
using Theater.Contracts;
using Theater.Contracts.Theater;
using Theater.Entities.Theater;

namespace Theater.Controllers.Admin
{
    [Route("api/admin/piece")]
    public sealed class PieceAdminController : BaseAdminController<IPieceService, PieceParameters, PieceEntity>
    {
        public PieceAdminController(IPieceService service) : base(service)
        {
        }

        /// <summary>
        /// Добавить дату для пьесы
        /// </summary>
        /// <response code="200">В случае успешного запроса</response>
        /// <response code="400">В случае ошибок валидации</response>
        // [Authorize(Policy = nameof(RoleModel.User.Policies.UserSearch))]
        [HttpPost("{pieceId:guid}/date")]
        [ProducesResponseType(typeof(DocumentMeta), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromRoute] Guid pieceId, [FromBody] PieceDateParameters parameters)
        {
            var pieceDateCreateResult = await Service.CreatePieceDate(pieceId, parameters.Date);

            return RenderResult(pieceDateCreateResult);
        }
    }
}