using System;

namespace Theater.Abstractions.Filters;

public sealed class PieceTicketFilterSettings : PagingSortSettings
{
    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public Guid UserId { get; set; }
}