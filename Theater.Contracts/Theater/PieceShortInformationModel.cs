using System;
using System.Collections.Generic;
using Theater.Contracts.FileStorage;

namespace Theater.Contracts.Theater;

public class PieceShortInformationModel
{
    /// <summary>
    /// Идентификатор пьесы
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Наименование пьесы
    /// </summary>
    public string PieceName { get; set; }

    /// <summary>
    /// Жанр пьесы
    /// </summary>
    public string PieceGenre { get; set; }

    /// <summary>
    /// Краткое описание пьесы
    /// </summary>
    public string ShortDescription { get; set; }

    /// <summary>
    /// Работники, которые принимали участие в пьесе
    /// </summary>
    public IReadOnlyCollection<TheaterWorkerShortInformationModel> WorkerShortInformation { get; set; }
        
    /// <summary>
    /// Даты
    /// </summary>
    public IReadOnlyCollection<PieceDateModel> PieceDates { get; set; }

    /// <summary>
    /// Основная фотография пьесы
    /// </summary>
    public StorageFileListItem MainPicture { get; set; }
}