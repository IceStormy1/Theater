using System;
using Theater.Common.Enums;
using Theater.Entities.Users;

namespace Theater.Entities.Rooms;

/// <summary>
/// Сообщение в чате
/// </summary>
public sealed class MessageEntity : BaseEntity, IHasCreatedAt, IHasUpdatedAt
{
    public MessageEntity()
    {
        
    }

    public MessageEntity(Guid userId, Guid roomId, string text, MessageType type)
    {
        UserId = userId;
        RoomId = roomId;
        Text = text;
        MessageType = type;
    }

    /// <summary>
    /// Текст сообщения
    /// </summary>
    public string Text { get; set; }

    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public Guid UserId { get; set; }

    /// <inheritdoc cref="UserEntity"/>
    public UserEntity User { get; set; }

    /// <inheritdoc cref="MessageType"/>
    public MessageType MessageType { get; set; }

    /// <inheritdoc cref="IHasCreatedAt.CreatedAt"/>
    public DateTime CreatedAt { get; set; }

    /// <inheritdoc cref="IHasUpdatedAt.UpdatedAt"/>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Идентификатор комнаты
    /// </summary>
    public Guid RoomId { get; set; }

    /// <inheritdoc cref="RoomEntity"/>
    public RoomEntity Room { get; set; }
}