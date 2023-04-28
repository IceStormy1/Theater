using System;

namespace Theater.Abstractions.Filter;

public class PieceFilterSettings : PagingSortSettings
{
    /// <summary>
    /// Идентификатор жанра
    /// </summary>
    public Guid? GenreId { get; set; }
}