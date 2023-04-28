using Theater.Contracts.Theater;
using Theater.Entities.Theater;

namespace Theater.Abstractions.PieceGenre;

public interface IPieceGenreService : ICrudService<PiecesGenreParameters, PiecesGenreEntity>
{
}