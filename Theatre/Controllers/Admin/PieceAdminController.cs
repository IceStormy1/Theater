using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Theater.Abstractions.Piece;
using Theater.Abstractions.UserAccount;
using Theater.Contracts.Theater.Piece;
using Theater.Controllers.Base;

namespace Theater.Controllers.Admin;

[Route("api/admin/piece")]
[SwaggerTag("Админ. Методы для работы с пьесами")]
public sealed class PieceAdminController : AdminBaseController<PieceParameters>
{
    public PieceAdminController(
        IMapper mapper, 
        IPieceService pieceService,
        IUserAccountService userAccountService) : base(pieceService, mapper, userAccountService)
    {
    }
}