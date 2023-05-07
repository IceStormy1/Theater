using System;

namespace Theater.Contracts.Theater.PiecesGenre;

public sealed class PiecesGenreModel : PiecesGenreParameters
{
    /// <summary>
    /// Идентификатор жанра
    /// </summary>
    public Guid Id { get; set; }
}