using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using Theater.Abstractions.PieceDates;
using Theater.Entities.Theater;

namespace Theater.Sql.Repositories;

public sealed class PieceRepository : BaseCrudRepository<PieceEntity>, IPieceRepository
{
    private readonly TheaterDbContext _dbContext;

    public PieceRepository(
        TheaterDbContext dbContext,
        ILogger<BaseCrudRepository<PieceEntity>> logger) : base(dbContext, logger)
    {
        _dbContext = dbContext;
    }

    public async Task<PieceEntity> GetPieceWithDates(Guid pieceId)
    {
        return await _dbContext.Pieces
            .AsNoTracking()
            .Include(x => x.PieceDates)
            .FirstOrDefaultAsync(x => x.Id == pieceId);
    }

    public override IQueryable<PieceEntity> AddIncludes(IQueryable<PieceEntity> query)
    {
        return query
            .Include(piece => piece.Genre)
            .Include(piece => piece.PieceDates)
            .Include(piece => piece.PieceWorkers)
            .ThenInclude(x => x.TheaterWorker)
            .ThenInclude(x => x.Position)
            .Include(x => x.UserReviews)
            .ThenInclude(x => x.User);
    }
}