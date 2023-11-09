using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Theater.Abstractions.Message;
using Theater.Entities.Rooms;
using Theater.Entities.Users;

namespace Theater.Sql.Repositories;

public sealed class UserRoomsRepository : BaseCrudRepository<UserRoomEntity>, IUserRoomsRepository
{
    public UserRoomsRepository(
        TheaterDbContext dbContext,
        ILogger<BaseCrudRepository<UserRoomEntity>> logger
        ) : base(dbContext, logger)
    {
    }

    public Task<List<UserRoomEntity>> GetRoomMembers(Guid roomId)
        => DbSet.AsNoTracking()
            .Include(x => x.Room)
            .Include(x => x.User)
            .Where(x => x.RoomId == roomId)
            .ToListAsync();

    public Task<List<UserEntity>> GetUsersNotInRoom(Guid roomId, IReadOnlyCollection<Guid> addedUsersToRoom)
        => DbContext.Users.AsNoTracking()
            .Where(x => addedUsersToRoom.Contains(x.Id) && x.UserRooms.All(c => c.RoomId != roomId))
            .ToListAsync();
}