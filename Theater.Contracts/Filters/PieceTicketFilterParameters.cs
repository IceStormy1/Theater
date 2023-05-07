using System;

namespace Theater.Contracts.Filters;

public sealed class PieceTicketFilterParameters : PagingSortParameters
{
    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public Guid UserId { get; set; }
}