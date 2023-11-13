using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Theater.Common;
using Theater.Contracts.Filters;
using Theater.Contracts.Rooms;

namespace Theater.Abstractions.Rooms;

public interface IRoomService : ICrudService<RoomParameters>
{
    /// <summary>
    /// Получение списка чатов с фильтрацией
    /// </summary>
    /// <param name="userId">Идентификатор текущего пользователя</param>
    /// <param name="filter">Фильтр для поиска чатов/контактов. Для индивидуальных контактов поиск осуществляется по имени или фамилии пользователя</param>
    /// <returns></returns>
    Task<Result<List<RoomItemDto>>> GetRoomsForUser(Guid userId, RoomSearchParameters filter);
}