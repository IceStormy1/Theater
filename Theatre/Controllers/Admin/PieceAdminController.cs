using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Theater.Abstractions.Piece;
using Theater.Contracts;
using Theater.Contracts.Theater;
using Theater.Policy;

namespace Theater.Controllers.Admin
{
    
    [Authorize]
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
        public async Task<IActionResult> CreatePiece(PieceParameters parameters)
        {
            var piecesShortInformation = await Service.CreatePiece(parameters);

            return RenderResult(piecesShortInformation);
        }

        // TODO: Update
        // TODO: Delete
    }
}