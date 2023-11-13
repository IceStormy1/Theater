using System.Collections.Generic;
using Theater.Common.Enums;

namespace Theater.Entities.Theater;

public sealed class WorkersPositionEntity : BaseEntity
{
    /// <summary>
    /// Наименование должности
    /// </summary>
    public string PositionName { get; set; }

    /// <summary>
    /// Тип должности
    /// </summary>
    public PositionType PositionType { get; set; }

    public List<TheaterWorkerEntity> TheaterWorker { get; set; }
}