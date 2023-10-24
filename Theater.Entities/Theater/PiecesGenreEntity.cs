using System.Collections.Generic;

namespace Theater.Entities.Theater;

public sealed class PiecesGenreEntity : BaseEntity
{
    /// <summary>
    /// Наименование роли
    /// </summary>
    public string GenreName { get; set; }

    public List<PieceEntity> Pieces { get; set; }
}