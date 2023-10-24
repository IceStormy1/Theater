using SignalRSwaggerGen.Attributes;
using SignalRSwaggerGen.Enums;

namespace Theater.SignalR;

[SignalRHub]
public interface IChatClient
{
    /// <summary>
    /// Событие о закрытии чата для текущего пользователя
    /// Нужен для обновления списка чатов, так как закрытый чат не должен быть отображен пользователю
    /// </summary>
    /// <param name="roomId">Идентификатор активной комнаты</param>
    /// <returns></returns>
    [SignalRMethod(
        summary: "Событие о закрытии чата для текущего пользователя",
        description: "Нужен для обновления списка чатов, так как закрытый чат не должен быть отображен пользователю",
        autoDiscover: AutoDiscover.Params)]
    Task OnRoomExit([SignalRParam(description: "Идентификатор активной комнаты")] long roomId);
}