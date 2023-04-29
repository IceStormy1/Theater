using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Theater.Abstractions.WorkersPosition;
using Theater.Common;
using Theater.Contracts.Theater;
using Theater.Entities.Theater;

namespace Theater.Sql.Repositories;

public sealed class WorkersPositionRepository : BaseCrudRepository<WorkersPositionEntity>, IWorkersPositionRepository
{
    public WorkersPositionRepository(
        TheaterDbContext dbContext,
        ILogger<BaseCrudRepository<WorkersPositionEntity>> logger) : base(dbContext, logger)
    {
    }

    public async Task<bool> HasTheaterWorkers(Guid workerPositionId)
    {
        return await DbContext.TheaterWorkers.AnyAsync(x => x.PositionId == workerPositionId);
    }

    public async Task<IReadOnlyCollection<WorkersPositionEntity>> GetWorkerPositions(PositionType? positionType)
    {
        var query = DbContext.WorkersPositions.AsNoTracking();

        if (positionType.HasValue)
            query = query.Where(x => x.PositionType == positionType.Value);

        return await query.ToListAsync();
    }
}