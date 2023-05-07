using System.Collections.Generic;
using System.Threading.Tasks;
using Theater.Contracts.Theater.PiecesGenre;

namespace Theater.Abstractions.PieceGenre;

public interface IPieceGenreService : ICrudService<PiecesGenreParameters>
{
    /// <summary>
    /// Возвращает все жанры пьес
    /// </summary>
    /// <returns></returns>
    Task<IReadOnlyCollection<PiecesGenreModel>> GetAllGenres();
}