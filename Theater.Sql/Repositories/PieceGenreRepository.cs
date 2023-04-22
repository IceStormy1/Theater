using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Theater.Abstractions.PieceGenre;
using Theater.Entities.Theater;

namespace Theater.Sql.Repositories
{
    public sealed class PieceGenreRepository : BaseCrudRepository<PiecesGenreEntity>, IPieceGenreRepository
    {
        public PieceGenreRepository(
            TheaterDbContext dbContext,
            ILogger<BaseCrudRepository<PiecesGenreEntity>> logger) : base(dbContext, logger)
        {
        }

        public async Task<bool> HasPieces(Guid genreId)
            => await DbContext.Pieces.AnyAsync(x => x.GenreId == genreId);
    }
}
