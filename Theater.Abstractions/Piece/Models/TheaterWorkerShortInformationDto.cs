using System;
using Theater.Common;

namespace Theater.Abstractions.Piece.Models;

public sealed class TheaterWorkerShortInformationDto
{
    /// <summary>
    /// Идентификатор работника театра
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Полное имя работника театра
    /// </summary>
    public string FullName { get; set; }

    /// <summary>
    /// Наименование должности работника театра
    /// </summary>
    public string PositionName { get; set; }

    /// <summary>
    /// Наименование типа должности
    /// </summary>
    public PositionType PositionTypeName { get; set; }
}