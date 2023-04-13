using AutoMapper;
using Microsoft.AspNetCore.Components;
using Theater.Abstractions.PieceWorkers;
using Theater.Contracts.Theater;
using Theater.Entities.Theater;

namespace Theater.Controllers.Admin
{
    [Route("api/admin/piece/attach")]
    public class PieceWorkersController : BaseAdminController<PieceWorkerParameters, PieceWorkerEntity>
    {
        public PieceWorkersController(
            IMapper mapper,
            IPieceWorkersService pieceWorkersService) : base(pieceWorkersService, mapper)
        {
        }
    }
}
