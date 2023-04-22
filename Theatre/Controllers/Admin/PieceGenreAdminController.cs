using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Theater.Abstractions.PieceGenre;
using Theater.Contracts;
using Theater.Contracts.Theater;
using Theater.Entities.Theater;

namespace Theater.Controllers.Admin
{
    [Route("api/admin/genre")]
    [ApiController]
    public class PieceGenreAdminController : BaseAdminController<PiecesGenreParameters, PiecesGenreEntity>
    {
        public PieceGenreAdminController(
            IPieceGenreService pieceGenreService,
            IMapper mapper) : base(pieceGenreService, mapper)
        {
        }
    }
}
