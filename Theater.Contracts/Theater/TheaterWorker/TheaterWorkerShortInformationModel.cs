using System;
using Theater.Common.Enums;
using Theater.Contracts.FileStorage;

namespace Theater.Contracts.Theater.TheaterWorker;

/// <summary>
/// Краткая информация о работнике театра
/// </summary>
public sealed class TheaterWorkerShortInformationModel
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
    /// Тип должности
    /// </summary>
    public PositionType PositionType { get; set; }

    /// <summary>
    /// Наименование типа должности
    /// </summary>
    public string PositionTypeName { get; set; }

    /// <summary>
    /// Основная фотография пьесы
    /// </summary>
    public StorageFileListItem MainPhoto { get; set; }
}