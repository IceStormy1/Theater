using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using Theater.Abstractions;
using Theater.Abstractions.PieceGenre;
using Theater.Contracts.Theater.PiecesGenre;
using Theater.Entities.Theater;

namespace Theater.Core.Theater.Services;

public sealed class PieceGenreService : BaseCrudService<PiecesGenreParameters, PiecesGenreEntity>, IPieceGenreService
{
    private readonly IPieceGenreRepository _pieceGenreRepository;

    public PieceGenreService(
        IMapper mapper,
        IPieceGenreRepository repository,
        ILogger<PieceGenreService> logger,
        IDocumentValidator<PiecesGenreParameters> documentValidator, IPieceGenreRepository pieceGenreRepository) : base(mapper, repository, documentValidator, logger)
    {
        _pieceGenreRepository = pieceGenreRepository;
    }

    public async Task<IReadOnlyCollection<PiecesGenreModel>> GetAllGenres()
    {
        var genresEntities = await _pieceGenreRepository.GetAllGenres();

        return Mapper.Map<IReadOnlyCollection<PiecesGenreModel>>(genresEntities);
    }
}