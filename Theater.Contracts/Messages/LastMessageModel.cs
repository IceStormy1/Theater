using System;
using Theater.Common.Enums;

namespace Theater.Contracts.Messages;

/// <summary>
/// Последнее сообщение в чате
/// </summary>
public sealed class LastMessageModel
{
    /// <summary>
    /// Автор сообщения
    /// </summary>
    public Guid AuthorId { get; set; }

    /// <summary>
    /// Текст сообщения
    /// </summary>
    public string Text { get; set; } = null!;

    /// <inheritdoc cref="MessageStatus"/>
    public MessageStatus Status { get; set; }
}