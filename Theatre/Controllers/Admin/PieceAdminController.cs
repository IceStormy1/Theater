using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Theater.Abstractions.Piece;
using Theater.Contracts.Theater;
using Theater.Entities.Theater;

namespace Theater.Controllers.Admin
{
    [Authorize]
    [Route("api/admin/piece")]
    public sealed class PieceAdminController : BaseAdminController<IPieceService, PieceParameters, PieceEntity>
    {
        public PieceAdminController(IPieceService service) : base(service)
        {
        }
    }
}