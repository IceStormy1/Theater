using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Theater.Abstractions.Piece.Models;
using Theater.Abstractions.TheaterWorker;
using Theater.Common;
using Theater.Entities.Theater;
using Theater.Abstractions.TheaterWorker.Models;

namespace Theater.Sql.Repositories
{
    public sealed class TheaterWorkerRepository : ITheaterWorkerRepository
    {
        private readonly TheaterDbContext _dbContext;

        public TheaterWorkerRepository(TheaterDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IReadOnlyDictionary<int, int>> GetTotalWorkers()
        {
            var totalWorkers = await _dbContext.TheaterWorkers
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
            var workersShortInformation = await _dbContext.TheaterWorkers
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

        public async Task<WriteResult<TheaterWorkerEntity>> GetTheaterWorkerById(Guid id)
        {
            var workersShortInformation = await _dbContext.TheaterWorkers
                .AsNoTracking()
                .Include(x => x.Position)
                .Include(x => x.PieceWorkers)
                .ThenInclude(x => x.Piece)
                .FirstOrDefaultAsync(x => x.Id == id);

            return workersShortInformation is null 
                ? WriteResult<TheaterWorkerEntity>.FromError(TheaterWorkerErrors.NotFound.Error)
                : WriteResult.FromValue(workersShortInformation);
        }
    }
}