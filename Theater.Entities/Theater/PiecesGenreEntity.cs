using System;
using System.Collections.Generic;

namespace Theater.Entities.Theater;

public sealed class PiecesGenreEntity : IEntity
{
    /// <summary>
    /// Идентификатор жанра
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Наименование роли
    /// </summary>
    public string GenreName { get; set; }

    public List<PieceEntity> Pieces { get; set; }
}