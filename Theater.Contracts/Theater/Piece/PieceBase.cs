using System;

namespace Theater.Contracts.Theater.Piece;

public class PieceBase
{
    /// <summary>
    /// Идентификатор пьесы
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Наименование пьесы
    /// </summary>
    public string PieceName { get; set; }
}