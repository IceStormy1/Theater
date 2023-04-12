using Microsoft.Extensions.Logging;
using Theater.Abstractions.Piece;
using Theater.Entities.Theater;

namespace Theater.Sql.Repositories
{
    public sealed class PieceDateRepository : BaseCrudRepository<PieceDateEntity>, IPieceDateRepository
    {
        public PieceDateRepository(
            TheaterDbContext dbContext,
            ILogger<BaseCrudRepository<PieceDateEntity>> logger) : base(dbContext, logger)
        {
        }
    }
}
