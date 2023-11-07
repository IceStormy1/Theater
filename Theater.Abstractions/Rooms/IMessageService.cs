using System;
using System.Threading.Tasks;
using Theater.Common;
using Theater.Contracts.Messages;

namespace Theater.Abstractions.Rooms;

public interface IMessageService
{
    /// <summary>
    /// Отправка нового сообщения
    /// </summary>
    /// <param name="roomId">Идентификатор активной комнаты</param>
    /// <param name="userId">Индентификатор текущего пользователя</param>
    /// <param name="newMessage">Тело нового сообщения</param>
    Task<Result<MessageModel>> SendMessage(Guid roomId, Guid userId, NewMessageModel newMessage);
}