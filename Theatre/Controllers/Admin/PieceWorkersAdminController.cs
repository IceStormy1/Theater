using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Theater.Abstractions.PieceWorkers;
using Theater.Contracts.Theater.PieceWorker;
using Theater.Controllers.Base;

namespace Theater.Controllers.Admin;

[Route("api/admin/pieceWorker")]
[SwaggerTag("Админ. Методы для работы с работниками театра, которые участвуют в пьесе")]
public sealed class PieceWorkersAdminController : AdminBaseController<PieceWorkerParameters>
{
    public PieceWorkersAdminController(
        IMapper mapper,
        IPieceWorkersService pieceWorkersService) : base(pieceWorkersService, mapper)
    {
    }
}