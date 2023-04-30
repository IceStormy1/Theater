using AutoMapper;
using Theater.Abstractions;
using Theater.Abstractions.PieceGenre;
using Theater.Contracts.Theater.PiecesGenre;
using Theater.Entities.Theater;

namespace Theater.Core.Theater.Services;

public sealed class PieceGenreService : ServiceBase<PiecesGenreParameters, PiecesGenreEntity>, IPieceGenreService
{
    public PieceGenreService(
        IMapper mapper,
        IPieceGenreRepository repository,
        IDocumentValidator<PiecesGenreParameters> documentValidator) : base(mapper, repository, documentValidator)
    {
    }
}