using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Theater.Abstractions.Filters;
using Theater.Abstractions.Rooms;
using Theater.Entities.Rooms;
using Theater.Entities.Users;

namespace Theater.Sql.Repositories;

public sealed class RoomRepository : BaseCrudRepository<RoomEntity>, IRoomRepository
{
    public RoomRepository(TheaterDbContext dbContext, ILogger<BaseCrudRepository<RoomEntity>> logger) : base(dbContext, logger)
    {
    }

    public Task<UserRoomEntity> GetActiveRoomRelationForUser(Guid userId, Guid roomId)
        => DbContext.UserRooms
            .AsNoTracking()
            .Include(x => x.User)
            .Include(x => x.Room)
            .FirstOrDefaultAsync(x => x.User.Id == userId
                                      && x.Room.Id == roomId
                                      && x.IsActive);

    public Task<RoomEntity> GetActiveRoomForUser(Guid userId, Guid roomId)
        => DbContext.Rooms
            .FirstOrDefaultAsync(x => x.Users.Any(c => c.UserId == userId 
                                                       && c.RoomId == roomId 
                                                       && c.IsActive));

    public Task<RoomEntity[]> GetRoomsForUser(Guid userId, RoomSearchSettings filter)
        => DbSet.AsNoTracking()
            .Include(x => x.Users)
                .ThenInclude(x => x.User)
            .Where(x => x.Users.Any(c => c.UserId == userId && c.IsActive))
            .Skip(filter.Offset)
            .Take(filter.Limit)
            .OrderByDescending(x => x.Messages.OrderByDescending(y => y.CreatedAt).First().CreatedAt)
            .ToArrayAsync();

    public Task<bool> IsMemberOfRoom(Guid userId, Guid roomId)
        => DbContext.UserRooms
            .AsNoTracking()
            .AnyAsync(x => x.User.Id == userId
                           && x.Room.Id == roomId
                           && x.IsActive);

    public async Task<Dictionary<Guid, UserEntity>> GetUsersByIndividualRooms(Guid userId, IList<Guid> roomIds)
    {
        if (roomIds.Count == 0)
            return new Dictionary<Guid, UserEntity>();

        return await DbContext.UserRooms
            .AsNoTracking()
            .Include(x => x.User)
            .Where(x => roomIds.Contains(x.RoomId) && x.UserId != userId)
            .Select(x => new { x.RoomId, x.User })
            .Distinct()
            .ToDictionaryAsync(x => x.RoomId, x => x.User);
    }
}