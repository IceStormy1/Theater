using System;
using System.Collections.Generic;
using Theater.Common.Enums;

namespace Theater.Entities.Rooms;

/// <summary>
/// Комната
/// </summary>
public sealed class RoomEntity : BaseEntity, IHasCreatedAt, IHasUpdatedAt
{
    /// <summary>
    /// Название комнаты
    /// </summary>
    /// <remarks>
    /// null, если <see cref="Type"/> == <see cref="Common.Enums.RoomType.Individual"/>
    /// </remarks>
    public string Title { get; set; }

    public RoomType Type { get; set; }

    /// <inheritdoc cref="IHasCreatedAt.CreatedAt"/>
    public DateTime CreatedAt { get; set; }

    /// <inheritdoc cref="IHasUpdatedAt.UpdatedAt"/>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Пользователи чата 
    /// </summary>
    public List<UserRoomEntity> Users { get; set; } = new();

    public List<MessageEntity> Messages { get; set; } = new();
}