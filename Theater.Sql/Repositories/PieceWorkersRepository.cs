using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using Theater.Abstractions.PieceWorkers;
using Theater.Entities.Theater;

namespace Theater.Sql.Repositories
{
    public class PieceWorkersRepository : BaseCrudRepository<PieceWorkerEntity>, IPieceWorkersRepository
    {
        private readonly TheaterDbContext _dbContext;

        public PieceWorkersRepository(
            TheaterDbContext dbContext,
            ILogger<BaseCrudRepository<PieceWorkerEntity>> logger) : base(dbContext, logger)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> CheckWorkerRelation(Guid theaterWorkerId, Guid pieceId)
        {
            return await _dbContext.PieceWorkers
                .AnyAsync(x => x.PieceId == pieceId && x.TheaterWorkerId == theaterWorkerId);
        }
    }
}
