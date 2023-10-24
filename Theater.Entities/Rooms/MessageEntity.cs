using System;
using Theater.Common.Enums;
using Theater.Entities.Users;

namespace Theater.Entities.Rooms;

/// <summary>
/// Сообщение в чате
/// </summary>
public sealed class MessageEntity : BaseEntity, IHasCreatedAt, IHasUpdatedAt
{
    /// <summary>
    /// Текст сообщения
    /// </summary>
    public string Text { get; set; }

    /// <inheritdoc cref="UserEntity"/>
    public UserEntity User { get; set; }

    /// <inheritdoc cref="MessageType"/>
    public MessageType MessageType { get; set; }

    /// <inheritdoc cref="IHasCreatedAt.CreatedAt"/>
    public DateTime CreatedAt { get; set; }

    /// <inheritdoc cref="IHasUpdatedAt.UpdatedAt"/>
    public DateTime? UpdatedAt { get; set; }

    /// <inheritdoc cref="RoomEntity"/>
    public RoomEntity Room { get; set; }
}