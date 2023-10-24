using System;
using System.Threading.Tasks;
using Theater.Abstractions.Filters;
using Theater.Entities.Rooms;

namespace Theater.Abstractions.Rooms;

public interface IRoomsRepository : ICrudRepository<RoomEntity>
{
    /// <summary>
    /// Получить связь юзера и активной комнаты
    /// </summary>
    /// <param name="userId">Проверяемый пользователь</param>
    /// <param name="roomId">Идентификатор комнаты</param>
    /// <returns></returns>
    Task<UserRoomEntity?> GetActiveRoomRelationForUser(Guid userId, Guid roomId);

    /// <summary>
    /// Получить список чатов юзера с пейджинацией
    /// </summary>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <param name="filter">Фильтр с пейджинацией. Для индивидуальных контактов поиск осуществляется по имени или фамилии пользователя</param>
    /// <returns></returns>
    Task<RoomEntity[]> GetRoomsForUser(Guid userId, RoomSearchSettings filter);
}