using System;
using Theater.Entities.Users;

namespace Theater.Entities.Theater;

public sealed class UserReviewEntity : BaseEntity, IHasCreatedAt, IHasUpdatedAt
{
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

    /// <inheritdoc cref="IHasCreatedAt.CreatedAt"/>
    public DateTime CreatedAt { get; set; }

    /// <inheritdoc cref="IHasUpdatedAt.UpdatedAt"/>
    public DateTime? UpdatedAt { get; set; }
}