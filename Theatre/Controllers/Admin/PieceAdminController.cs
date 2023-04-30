using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Theater.Abstractions.Piece;
using Theater.Contracts.Theater.Piece;
using Theater.Controllers.BaseControllers;

namespace Theater.Controllers.Admin;

[Route("api/admin/piece")]
[SwaggerTag("Админ. Методы для работы с пьесами")]
public sealed class PieceAdminController : AdminBaseController<PieceParameters>
{
    public PieceAdminController(
        IMapper mapper, 
        IPieceService pieceService) : base(pieceService, mapper)
    {
    }
}