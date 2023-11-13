using System;
using Theater.Common.Enums;

namespace Theater.Contracts.Messages;

public sealed class MessageModel
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
    public AuthorDto Author { get; set; }

    /// <summary>
    /// Дата создания
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <inheritdoc cref="MessageStatus"/>
    public MessageStatus Status { get; set; }
}