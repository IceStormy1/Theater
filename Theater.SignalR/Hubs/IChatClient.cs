using SignalRSwaggerGen.Attributes;
using Theater.Contracts.Messages;

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
    /// <returns></returns>
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
    /// Update user list
    /// </summary>
    Task UpdateUsersAsync(List<Guid> usersIds);
}