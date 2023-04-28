using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Theater.Abstractions.PieceWorkers;
using Theater.Contracts.Theater;
using Theater.Controllers.BaseControllers;
using Theater.Entities.Theater;

namespace Theater.Controllers.Admin;

[Route("api/admin/pieceWorker")]
public class PieceWorkersAdminController : AdminBaseController<PieceWorkerParameters, PieceWorkerEntity>
{
    public PieceWorkersAdminController(
        IMapper mapper,
        IPieceWorkersService pieceWorkersService) : base(pieceWorkersService, mapper)
    {
    }
}