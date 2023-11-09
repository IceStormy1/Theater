using System;
using System.Threading.Tasks;
using Theater.Common;
using Theater.Contracts.Rooms;

namespace Theater.Abstractions.Rooms;

public interface IUserRoomService
{
    /// <summary>
    /// Покинуть чат
    /// </summary>
    /// <param name="userId">Идентификатор текущего пользователя</param>
    /// <param name="roomId">Идентификатор комнаты</param>
    Task<Result> LeaveRoom(Guid userId, Guid roomId);

    /// <summary>
    /// Добавляет пользователей в комнату
    /// </summary>
    Task<Result> InviteUsersToRoom(Guid userId, Guid roomId, InviteUsersModel inviteUsersModel);
}