using System;

namespace Theater.Contracts.Rabbit;

public sealed class UserJoinedModel
{
    /// <summary>
    /// Идентификатор комнаты
    /// </summary>
    public Guid RoomId { get; set; }

    /// <summary>
    /// Идентификатор пользователя, который был добавлен
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Название комнаты
    /// </summary>
    public string RoomTitle { get; set; }
}