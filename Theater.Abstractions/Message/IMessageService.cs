using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Theater.Common;
using Theater.Contracts.Filters;
using Theater.Contracts.Messages;
using Theater.Entities.Rooms;

namespace Theater.Abstractions.Message;

public interface IMessageService
{
    /// <summary>
    /// Отправка нового сообщения
    /// </summary>
    /// <param name="roomId">Идентификатор активной комнаты</param>
    /// <param name="userId">Индентификатор текущего пользователя</param>
    /// <param name="newMessage">Тело нового сообщения</param>
    Task<Result<MessageModel>> SendMessage(Guid roomId, Guid userId, NewMessageModel newMessage);

    /// <summary>
    /// Получение списка сообщений чата с пагинацией
    /// </summary>
    /// <param name="roomId">Идентификатор активной комнаты</param>
    /// <param name="userId">Индентификатор текущего пользователя</param>
    /// <param name="filter">Параметры пагинации</param>
    Task<Result<List<MessageModel>>> GetMessages(Guid roomId, Guid userId, MessageFilterParameters filter);

    /// <summary>
    /// Отправляет сообытие об отправленном сообщении
    /// </summary>
    /// <param name="roomId">Идентификатор активной комнаты</param>
    /// <param name="userId">Индентификатор текущего пользователя</param>
    /// <param name="message">Сообщение</param>
    Task PublishMessageSent(Guid userId, Guid roomId, MessageEntity message);
}