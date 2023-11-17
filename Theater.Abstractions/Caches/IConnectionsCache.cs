using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Theater.Abstractions.Caches;

public interface IConnectionsCache
{
    /// <summary>
    /// Добавить соединение для пользователя
    /// </summary>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <param name="connectionId">Идентификатор соединения</param>
    Task SetConnection(Guid userId, string connectionId);

    /// <summary>
    /// Получить соединения пользователя
    /// </summary>
    /// <param name="userId"></param>
    Task<HashSet<string>> GetConnections(Guid userId);

    /// <summary>
    /// Удалить соединения для пользователя
    /// </summary>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <param name="connectionId">Идентификатор соединения</param>
    Task RemoveConnection(Guid userId, string connectionId);

    /// <summary>
    /// Получить соединения по идентификаторам пользователей
    /// </summary>
    /// <param name="userIds">Идентификаторы пользователей</param>
    Task<IDictionary<Guid, HashSet<string>>> GetUserConnectionsByIds(IEnumerable<Guid> userIds);

    /// <summary>
    /// Очистить все соединения для пользователя
    /// </summary>
    /// <param name="userId">Идентификатор пользователя</param>
    Task ClearConnections(Guid userId);
}