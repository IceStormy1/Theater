using System;
using System.Collections.Generic;
using Theater.Common.Enums;
using Theater.Contracts.Messages;
using Theater.Contracts.UserAccount;

namespace Theater.Contracts.Rooms;

/// <summary>
/// Комната
/// </summary>
public sealed class RoomItemDto
{
    /// <summary>
    /// Идентификатор комнаты
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Название комнаты
    /// </summary>
    public string Title { get; set; }

    // TODO: Room avatar

    /// <summary>
    /// Тип комнаты (0-Individual, 1-Group)
    /// </summary>
    public RoomType RoomType { get; set; }

    /// <inheritdoc cref="LastMessageModel"/>
    public LastMessageModel LastMessage { get; set; } = null!;

    /// <summary>
    /// Пользователи комнаты
    /// </summary>
    public IReadOnlyCollection<UserShortItem> Users { get; set; }
}