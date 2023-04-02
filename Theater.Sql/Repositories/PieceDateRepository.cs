using Microsoft.Extensions.Logging;
using Theater.Entities.Theater;

namespace Theater.Sql.Repositories
{
    public sealed class PieceDateRepository : BaseCrudRepository<PieceDateEntity, TheaterDbContext>, IPieceDateRepository
    {
        public PieceDateRepository(
            TheaterDbContext dbContext,
            ILogger<BaseCrudRepository<PieceDateEntity, TheaterDbContext>> logger) : base(dbContext, logger)
        {
        }
    }
}
