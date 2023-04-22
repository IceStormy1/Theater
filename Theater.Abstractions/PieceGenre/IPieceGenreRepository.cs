using System;
using System.Threading.Tasks;
using Theater.Entities.Theater;

namespace Theater.Abstractions.PieceGenre
{
    public interface IPieceGenreRepository : ICrudRepository<PiecesGenreEntity>
    {
        /// <summary>
        /// Проверяет, имеет ли связь жанр с какой-нибудь пьесой
        /// </summary>
        /// <param name="genreId">Идентификатор жанра</param>
        Task<bool> HasPieces(Guid genreId);
    }
}