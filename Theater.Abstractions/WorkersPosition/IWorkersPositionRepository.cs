using System;
using System.Threading.Tasks;
using Theater.Entities.Theater;

namespace Theater.Abstractions.WorkersPosition;

public interface IWorkersPositionRepository : ICrudRepository<WorkersPositionEntity>
{
    /// <summary>
    /// Проверяет, имеется ли связь с работниками театра
    /// </summary>
    /// <param name="workerPositionId"></param>
    /// <returns></returns>
    Task<bool> HasTheaterWorkers(Guid workerPositionId);
}