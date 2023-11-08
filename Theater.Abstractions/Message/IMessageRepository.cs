using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Theater.Abstractions.Filters;
using Theater.Entities.Rooms;

namespace Theater.Abstractions.Message;

public interface IMessageRepository : ICrudRepository<MessageEntity>
{
    Task<List<MessageEntity>> GetMessages(Guid roomId, MessageFilterSettings filter);

    /// <summary>
    /// Получить список последних сообщений пользователя для указанных комнат с группировкой по идентификатору комнаты
    /// </summary>
    /// <param name="roomIds">Список идентификаторов комнат</param>
    /// <returns>Возвращает словарь ид комнаты -> последнее сообщение в комнате</returns>
    Task<IDictionary<Guid, MessageEntity>> GetLastMessagesForRooms(IEnumerable<Guid> roomIds);
}