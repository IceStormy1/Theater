using System;
using System.Collections.Generic;

namespace Theater.Abstractions.Piece.Models;

public class PieceShortInformationDto
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
    /// Работники, которые принимали участие в пьесе
    /// </summary>
    public IReadOnlyCollection<TheaterWorkerShortInformationDto> WorkerShortInformation { get; set; }
        
    /// <summary>
    /// Даты пьесы
    /// </summary>
    public IReadOnlyCollection<PieceDateDto> PieceDates { get; set; }
}