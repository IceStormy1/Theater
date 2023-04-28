using System;

namespace Theater.Contracts.Theater;

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
    /// Наименование типа должности
    /// </summary>
    public string PositionTypeName { get; set; }
}