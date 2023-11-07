using System;
using Theater.Common.Enums;

namespace Theater.Contracts.Rabbit;

/// <summary>
/// Модель отправки сообщения
/// </summary>
public sealed class MessageSentModel
{
    /// <summary>
    /// Уникальный Id сообщения
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Текст сообщения
    /// </summary>
    public string Text { get; set; } = null!;

    /// <inheritdoc cref="Common.Enums.MessageType"/>
    public MessageType MessageType { get; set; }

    /// <summary>
    /// Автор сообщения
    /// </summary>
    public Guid AuthorId { get; set; }

    /// <summary>
    /// Дата создания
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Идентификатор чата
    /// </summary>
    public Guid RoomId { get; set; }
}