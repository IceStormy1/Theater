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
    public sealed class PieceRepository : BaseCrudRepository<PieceEntity, TheaterDbContext>, IPieceRepository
    {
        public PieceRepository(
            TheaterDbContext dbContext,
            ILogger<BaseCrudRepository<PieceEntity, TheaterDbContext>> logger) : base(dbContext, logger)
        {
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
                    PieceDates = x.PieceDates.Select(c => new PieceDateDto { Id = c.Id, Date = c.Date }).ToList(),
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

        public async Task<PieceDto> GetPieceDtoById(Guid pieceId)
        {
            var pieceQuery = GetPieceQueryWithIncludes(pieceId);

            return await pieceQuery
                .AsNoTracking()
                .Select(x => new PieceDto
                {
                    Id = x.Id,
                    PieceGenre = x.Genre.GenreName,
                    PieceName = x.PieceName,
                    Description = x.Description,
                    ShortDescription = x.ShortDescription,
                    PhotoIds = x.PhotoIds,
                    PieceDates = x.PieceDates.Select(c => new PieceDateDto { Date = c.Date }).ToList(),
                    WorkerShortInformation = x.PieceWorkers.Select(c => new TheaterWorkerShortInformationDto
                    {
                        FullName = c.TheaterWorker.LastName + " " + c.TheaterWorker.FirstName + " " + c.TheaterWorker.LastName,
                        Id = c.TheaterWorkerId,
                        PositionName = c.TheaterWorker.Position.PositionName,
                        PositionTypeName = c.TheaterWorker.Position.PositionType
                    }).ToList()
                })
                .FirstOrDefaultAsync();
        }

        public async Task<PieceEntity> GetPieceWithDates(Guid pieceId)
        {
            return await DbContext.Pieces
                .AsNoTracking()
                .Include(x => x.PieceDates)
                .FirstOrDefaultAsync(x => x.Id == pieceId);
        }

        private IQueryable<PieceEntity> GetPieceQueryWithIncludes(Guid? pieceId = null)
        {
            var pieceQuery = AddIncludes(query: DbContext.Pieces.AsQueryable()).AsQueryable();

            if(pieceId.HasValue)
                pieceQuery = pieceQuery.Where(x=>x.Id == pieceId.Value);

            return pieceQuery;
        }

        protected override IQueryable<PieceEntity> AddIncludes(IQueryable<PieceEntity> query)
        {
            return query
                .Include(piece => piece.PieceDates)
                .Include(piece => piece.Genre)
                .Include(piece => piece.PieceWorkers)
                .ThenInclude(x => x.TheaterWorker)
                .ThenInclude(x => x.Position);
        }
    }
}
