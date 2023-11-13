using System;

namespace Theater.Contracts.Rooms;

public sealed class ReadMessageModel
{
    public Guid RoomId { get; set; }
    public Guid MessageId { get; set; }
}