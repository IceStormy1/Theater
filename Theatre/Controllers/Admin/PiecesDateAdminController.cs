using Microsoft.AspNetCore.Mvc;
using Theater.Abstractions.Piece;
using Theater.Contracts.Theater;
using Theater.Entities.Theater;

namespace Theater.Controllers.Admin
{
    [Route("api/admin/piece/date")]
    public class PiecesDateAdminController : BaseAdminController<PieceDateParameters, PieceDateEntity>
    {
        public PiecesDateAdminController(IPieceDateService service) : base(service)
        {
        }
    }
}
