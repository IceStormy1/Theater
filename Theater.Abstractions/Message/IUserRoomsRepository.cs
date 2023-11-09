using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Theater.Entities.Rooms;
using Theater.Entities.Users;

namespace Theater.Abstractions.Message;

public interface IUserRoomsRepository : ICrudRepository<UserRoomEntity>
{
    /// <summary>
    /// Возвращает пользователей чата
    /// </summary>
    /// <param name="roomId">Идентификатор комнаты</param>
    Task<List<UserRoomEntity>> GetRoomMembers(Guid roomId);

    /// <summary>
    /// Возвращает сущности пользователей, которые не состоят в чате
    /// </summary>
    /// <param name="roomId">Идентификатор комнаты</param>
    /// <param name="addedUsersToRoom">Добавляемые в комнату пользователи</param>
    Task<List<UserEntity>> GetUsersNotInRoom(Guid roomId, IReadOnlyCollection<Guid> addedUsersToRoom);
}