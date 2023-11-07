using MassTransit;
using SignalRSwaggerGen.Attributes;
using SignalRSwaggerGen.Enums;
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
    /// Update user list
    /// </summary>
    Task UpdateUsersAsync(List<Guid> usersIds);

    /// <summary>
    /// Send message
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    Task SendMessageAsync(string userId, string message);
}