using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Theater.Abstractions.PieceGenre;
using Theater.Contracts.Theater.PiecesGenre;
using Theater.Controllers.Base;

namespace Theater.Controllers.Admin;

[Route("api/admin/genre")]
[ApiController]
[SwaggerTag("Админ. Методы для работы с жанрами пьес")]
public sealed class PieceGenreAdminController : AdminBaseController<PiecesGenreParameters>
{
    public PieceGenreAdminController(
        IPieceGenreService pieceGenreService,
        IMapper mapper) : base(pieceGenreService, mapper)
    {
    }
}