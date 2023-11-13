using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Theater.Common.Enums;
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

    /// <summary>
    /// Возвращает все должности работников театра
    /// </summary>
    /// <param name="positionType">
    /// Тип должности. Опциональный параметр
    /// </param>
    Task<IReadOnlyCollection<WorkersPositionEntity>> GetWorkerPositions(PositionType? positionType);
}