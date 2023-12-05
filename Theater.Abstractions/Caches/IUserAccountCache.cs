using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Theater.Abstractions.Caches;

public interface IUserAccountCache
{
    /// <summary>
    /// Связывает внутренний и внешний идентификатор пользователя
    /// </summary>
    /// <param name="externalUserId">Идентификатор пользователя</param>
    /// <param name="innerUserId">Идентификатор соединения</param>
    Task LinkUserIds(Guid externalUserId, Guid innerUserId);

    /// <summary>
    /// Получить внутренний идентификатор по внешнему идентификатору пользователя
    /// </summary>
    /// <param name="externalUserId">Внешний идентификатор пользователя</param>
    Task<Guid?> GetInnerUserIdByExternalId(Guid externalUserId);
}