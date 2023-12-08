using System;
using Theater.Contracts.FileStorage;
using Theater.Contracts.UserAccount;

namespace Theater.Contracts.Theater.TheaterWorker;

public class TheaterWorkerParameters : UserBase
{
    /// <summary>
    /// Описание работника
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Идентификатор должности работника театра
    /// </summary>
    public Guid PositionId { get; set; }

    /// <summary>
    /// Основная фотография пьесы
    /// </summary>
    public StorageFileListItem MainPhoto { get; set; }
}