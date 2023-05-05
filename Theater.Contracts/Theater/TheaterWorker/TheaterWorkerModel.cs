using System;
using Theater.Common;

namespace Theater.Contracts.Theater.TheaterWorker;

public sealed class TheaterWorkerModel : TheaterWorkerParameters
{
    /// <summary>
    /// Идентификатор работника театра
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Наименование должности работника театра
    /// </summary>
    public string PositionName { get; set; }

    /// <summary>
    /// Тип должности
    /// </summary>
    public PositionType PositionType { get; set; }

    /// <summary>
    /// Наименование типа должности
    /// </summary>
    public string PositionTypeName { get; set; }
}