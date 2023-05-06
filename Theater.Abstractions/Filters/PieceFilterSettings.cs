using System;

namespace Theater.Abstractions.Filters;

public class PieceFilterSettings : PagingSortSettings
{
    /// <summary>
    /// Идентификатор жанра
    /// </summary>
    public Guid? GenreId { get; set; }

    /// <summary>
    /// Дата на которую нужно выбрать пьесы
    /// </summary>
    public DateTime? Date { get; set; }
}