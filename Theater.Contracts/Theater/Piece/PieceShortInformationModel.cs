using System.Collections.Generic;
using Theater.Contracts.FileStorage;
using Theater.Contracts.Theater.PieceDate;
using Theater.Contracts.Theater.TheaterWorker;

namespace Theater.Contracts.Theater.Piece;

public class PieceShortInformationModel : PieceBase
{
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