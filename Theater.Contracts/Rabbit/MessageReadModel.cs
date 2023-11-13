using System;

namespace Theater.Contracts.Rabbit;

public sealed class MessageReadModel
{
    public MessageReadModel(
        Guid roomId, 
        Guid messageId,
        Guid messageAuthorId, 
        DateTime messageTime)
    {
        RoomId = roomId;
        MessageId = messageId;
        MessageAuthorId = messageAuthorId;
        MessageTime = messageTime;
    }

    /// <summary>
    /// Идентификатор комнаты
    /// </summary>
    public Guid RoomId { get; }

    /// <summary>
    /// Идентификатор прочитанного сообщения
    /// </summary>
    public Guid MessageId { get; }

    /// <summary>
    /// Дата и время прочтения
    /// </summary>
    public DateTime MessageTime { get;  }
    
    /// <summary>
    /// Автор сообщения
    /// </summary>
    public Guid MessageAuthorId { get; }
}