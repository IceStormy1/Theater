using System;
using Theater.Entities.Authorization;

namespace Theater.Entities.Theater;

public sealed class UserReviewEntity : IEntity
{
    /// <summary>
    /// Идентификатор роли
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Текст рецензии
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Заголовок рецензии
    /// </summary>
    public string Title { get; set; }
        
    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Ссылка на пользователя
    /// </summary>
    public UserEntity User { get; set; }

    /// <summary>
    /// Идентификатор пьесы
    /// </summary>
    public Guid PieceId { get; set; }

    /// <summary>
    /// Ссылка на пьесу
    /// </summary>
    public PieceEntity Piece { get; set; }
}