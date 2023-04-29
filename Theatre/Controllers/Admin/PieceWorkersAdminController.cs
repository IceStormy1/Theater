using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Theater.Abstractions.PieceWorkers;
using Theater.Contracts.Theater;
using Theater.Controllers.BaseControllers;
using Theater.Entities.Theater;

namespace Theater.Controllers.Admin;

[Route("api/admin/pieceWorker")]
[SwaggerTag("Админ. Методы для работы с работниками театра, которые участвуют в пьесе")]
public class PieceWorkersAdminController : AdminBaseController<PieceWorkerParameters, PieceWorkerEntity>
{
    public PieceWorkersAdminController(
        IMapper mapper,
        IPieceWorkersService pieceWorkersService) : base(pieceWorkersService, mapper)
    {
    }
}