using System;

namespace Theater.Contracts.Theater.UserReview;

public sealed class UserReviewModel : UserReviewParameters
{
    /// <summary>
    /// Идентификатор рецензии
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Имя пользователя
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// Наименование пьесы
    /// </summary>
    public string PieceName { get; set; }
}