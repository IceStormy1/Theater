using System;

namespace Theater.Contracts.Filters;

public sealed class PieceFilterParameters : PagingSortParameters
{
    /// <summary>
    /// Идентификатор жанра
    /// </summary>
    public int? GenreId { get; set; }

    /// <summary>
    /// Дата на которую нужно выбрать пьесы
    /// </summary>
    public DateTime? Date { get; set; }
}