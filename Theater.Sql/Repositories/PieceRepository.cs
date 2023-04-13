using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Theater.Abstractions.Piece;
using Theater.Abstractions.Piece.Models;
using Theater.Entities.Theater;

namespace Theater.Sql.Repositories
{
    public sealed class PieceRepository : BaseCrudRepository<PieceEntity>, IPieceRepository
    {
        private readonly TheaterDbContext _dbContext;

        public PieceRepository(
            TheaterDbContext dbContext,
            ILogger<BaseCrudRepository<PieceEntity>> logger) : base(dbContext, logger)
        {
            _dbContext = dbContext;
        }

        public async Task<IReadOnlyCollection<PieceShortInformationDto>> GetPiecesShortInformation()
        {
            var pieceQuery = GetPieceQueryWithIncludes();

            return await pieceQuery
                .AsNoTracking()
                .Where(x => x.PieceDates.Any(c => c.Date >= DateTime.UtcNow))
                .Select(x => new PieceShortInformationDto
                {
                    Id = x.Id,
                    PieceGenre = x.Genre.GenreName,
                    PieceName = x.PieceName,
                    PieceDates = x.PieceDates.Select(c => new PieceDateDto { Id = c.Id, Date = c.Date, PieceId = c.PieceId}).ToList(),
                    WorkerShortInformation = x.PieceWorkers.Select(c => new TheaterWorkerShortInformationDto
                    {
                        FullName = $"{c.TheaterWorker.LastName} {c.TheaterWorker.FirstName} {c.TheaterWorker.LastName}",
                        Id = c.TheaterWorkerId,
                        PositionName = c.TheaterWorker.Position.PositionName,
                        PositionTypeName = c.TheaterWorker.Position.PositionType
                    }).ToList()
                })
                .ToListAsync();
        }

        public async Task<PieceEntity> GetPieceWithDates(Guid pieceId)
        {
            return await _dbContext.Pieces
                .AsNoTracking()
                .Include(x => x.PieceDates)
                .FirstOrDefaultAsync(x => x.Id == pieceId);
        }

        private IQueryable<PieceEntity> GetPieceQueryWithIncludes(Guid? pieceId = null)
        {
            var pieceQuery = AddIncludes(query: _dbContext.Pieces.AsQueryable()).AsQueryable();

            if(pieceId.HasValue)
                pieceQuery = pieceQuery.Where(x=>x.Id == pieceId.Value);

            return pieceQuery.AsNoTracking();
        }

        public override IQueryable<PieceEntity> AddIncludes(IQueryable<PieceEntity> query)
        {
            return query
                .Include(piece => piece.PieceDates)
                    .ThenInclude(x => x.PiecesTickets)
                .Include(piece => piece.Genre)
                .Include(piece => piece.PieceWorkers)
                .ThenInclude(x => x.TheaterWorker)
                .ThenInclude(x => x.Position);
        }
    }
}
