using SignalRSwaggerGen.Attributes;
using SignalRSwaggerGen.Enums;
using Theater.Contracts.Messages;
using Theater.Contracts.Rooms;

namespace Theater.SignalR.Hubs;

[SignalRHub]
public interface IChatClient
{
    Task OnMessageReceived(
        [SignalRParam(description: "Идентификатор активной комнаты")]
        Guid roomId,
        [SignalRParam(description: "Полученное сообщение")]
        MessageModel message);

    /// <summary>
    /// Событие об успешно доставленном сообщении
    /// </summary>
    /// <param name="roomId">Идентификатор активной комнаты</param>
    /// <param name="message">Доставленное сообщение</param>
    Task OnMessageDelivered(
        [SignalRParam(description: "Идентификатор активной комнаты")]
        Guid roomId,
        [SignalRParam(description: "Доставленное сообщение")]
        MessageModel message);

    /// <summary>
    /// Событие об открытии нового чата или добавлении текущего пользователя в чат
    /// Нужен для обновления списка чатов, так как новый чат должен быть отображен пользователю
    /// </summary>
    /// <param name="roomId">Идентификатор активной комнаты</param>
    /// <param name="title">Название комнаты</param>
    Task OnRoomEnter([SignalRParam(description: "Идентификатор активной комнаты")] Guid roomId, string title);

    /// <summary>
    /// Событие о выходе пользователя из чата.
    /// Нужен для обновления списка чатов, так как чат из которого вышел пользователь
    /// не должен быть отображен пользователю
    /// </summary>
    /// <param name="roomId">Идентификатор активной комнаты</param>
    Task OnRoomExit([SignalRParam(description: "Идентификатор активной комнаты")] Guid roomId);

    /// <summary>
    /// Обновляет список пользователей
    /// </summary>
    Task UpdateUsersAsync(List<Guid> usersIds);

    /// <summary>
    /// Событие о прочитанном сообщение в чате
    /// </summary>
    /// <param name="message">Прочитанное сообщение</param>
    [SignalRMethod(
        summary: "Событие о прочитанном сообщение в чате",
        description: "Нужен для пометки сообщение как прочитанное в ленте сообщений чата",
        autoDiscover: AutoDiscover.Params)]
    Task OnMessageRead(ReadMessageModel message);
}