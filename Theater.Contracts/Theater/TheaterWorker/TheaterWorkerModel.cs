using System;
using System.Collections.Generic;
using Theater.Common;
using Theater.Contracts.Theater.Piece;

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

    /// <summary>
    /// ФИО
    /// </summary>
    public string FullName => $"{LastName} {FirstName} {MiddleName}";

    /// <summary>
    /// Пьесы, в которых участовал актер
    /// </summary>
    public IReadOnlyCollection<PieceBase> Pieces { get; set; }
}