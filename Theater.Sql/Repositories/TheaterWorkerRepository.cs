using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Theater.Abstractions.Piece.Models;
using Theater.Abstractions.TheaterWorker;
using Theater.Entities.Theater;

namespace Theater.Sql.Repositories
{
    public sealed class TheaterWorkerRepository : BaseCrudRepository<TheaterWorkerEntity, TheaterDbContext>, ITheaterWorkerRepository
    {
        public TheaterWorkerRepository(TheaterDbContext dbContext, ILogger<BaseCrudRepository<TheaterWorkerEntity, TheaterDbContext>> logger) : base(dbContext, logger)
        {
        }

        public async Task<IReadOnlyDictionary<int, int>> GetTotalWorkers()
        {
            var totalWorkers = await DbContext.TheaterWorkers
                .AsNoTracking()
                .Include(x => x.Position)
                .GroupBy(x => x.Position.PositionType)
                .Select(x => new
                {
                    Total = x.Count(),
                    x.Key
                }).ToDictionaryAsync(x => (int)x.Key, v => v.Total);
         
            return totalWorkers;
        }

        public async Task<IReadOnlyCollection<TheaterWorkerShortInformationDto>> GetShortInformationWorkersByPositionType(int positionType)
        {
            var workersShortInformation = await DbContext.TheaterWorkers
                .AsNoTracking()
                .Include(x => x.Position)
                .Where(x=>(int)x.Position.PositionType == positionType)
                .Select(x => new TheaterWorkerShortInformationDto
                {
                    FullName = x.LastName + " " + x.FirstName + " " + x.LastName,
                    Id = x.Id,
                    PositionName = x.Position.PositionName,
                    PositionTypeName = x.Position.PositionType
                })
                .ToListAsync();

            return workersShortInformation;
        }

        protected override IQueryable<TheaterWorkerEntity> AddIncludes(IQueryable<TheaterWorkerEntity> query)
        {
            return query
                .Include(x => x.Position)
                .Include(x => x.PieceWorkers)
                .ThenInclude(x => x.Piece);
        }
    }
}