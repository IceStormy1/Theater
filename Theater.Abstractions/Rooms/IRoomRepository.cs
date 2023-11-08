using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Theater.Abstractions.Filters;
using Theater.Entities.Rooms;
using Theater.Entities.Users;

namespace Theater.Abstractions.Rooms;

public interface IRoomRepository : ICrudRepository<RoomEntity>
{
    /// <summary>
    /// Получить связь юзера и активной комнаты
    /// </summary>
    /// <param name="userId">Проверяемый пользователь</param>
    /// <param name="roomId">Идентификатор комнаты</param>
    /// <returns></returns>
    Task<UserRoomEntity> GetActiveRoomRelationForUser(Guid userId, Guid roomId);

    /// <summary>
    /// Получить комнату по идентификатору пользователя и комнаты
    /// </summary>
    /// <param name="userId">Проверяемый пользователь</param>
    /// <param name="roomId">Идентификатор комнаты</param>
    Task<RoomEntity> GetActiveRoomForUser(Guid userId, Guid roomId);

    /// <summary>
    /// Получить список чатов юзера с пейджинацией
    /// </summary>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <param name="filter">Фильтр с пейджинацией. Для индивидуальных контактов поиск осуществляется по имени или фамилии пользователя</param>
    /// <returns></returns>
    Task<RoomEntity[]> GetRoomsForUser(Guid userId, RoomSearchSettings filter);

    /// <summary>
    /// true - является участником чата
    /// </summary>
    /// <param name="userId">Проверяемый пользователь</param>
    /// <param name="roomId">Идентификатор комнаты</param>
    Task<bool> IsMemberOfRoom(Guid userId, Guid roomId);

    /// <summary>
    /// Получить список имен пользователей для индивидуальных чатов с группировкой по идентификатору комнаты
    /// </summary>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <param name="roomIds">Список идентификаторов индивидуальных комнат</param>
    /// <returns>Возвращает словарь ид комнаты -> имя пользователя</returns>
    Task<Dictionary<Guid, UserEntity>> GetUsersByIndividualRooms(Guid userId, IList<Guid> roomIds);
}