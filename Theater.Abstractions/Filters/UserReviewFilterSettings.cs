using System;

namespace Theater.Abstractions.Filters;

public sealed class UserReviewFilterSettings : PagingSortSettings
{
    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public Guid? UserId { get; set; }

    /// <summary>
    /// Идентификатор пьесы
    /// </summary>
    public Guid? PieceId { get; set; }
}