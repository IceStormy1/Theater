using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using Theater.Abstractions.Filters;
using Theater.Abstractions.Rooms;
using Theater.Common.Enums;
using Theater.Entities.Rooms;

namespace Theater.Sql.Repositories;

public sealed class RoomsRepository : BaseCrudRepository<RoomEntity>, IRoomsRepository
{
    public RoomsRepository(TheaterDbContext dbContext, ILogger<BaseCrudRepository<RoomEntity>> logger) : base(dbContext, logger)
    {
    }

    public Task<UserRoomEntity> GetActiveRoomRelationForUser(Guid userId, Guid roomId)
        => DbContext.UserRooms
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.User.Id == userId 
                                      && x.IsActive 
                                      && x.Room.Id == roomId);

    public async Task<RoomEntity[]> GetRoomsForUser(Guid userId, RoomSearchSettings filter)
    {
        var items = await DbContext.UserRooms.Where(userRoom => userRoom.User.Id == userId && userRoom.IsActive)
            .AsNoTracking()
            .Where(t =>
                (t.Room.RoomType == RoomType.Group && EF.Functions.ILike(t.Room.Title, $"%{filter.Query}%")) ||
                (t.Room.RoomType == RoomType.Individual &&
                 (EF.Functions.ILike(t.User.FirstName, $"%{filter.Query}%") ||
                  EF.Functions.ILike(t.User.LastName, $"%{filter.Query}%"))))
            .AsNoTracking()
            .Skip(filter.Offset)
            .Take(filter.Limit)
            .OrderByDescending(x => x.Room.Messages.OrderByDescending(y => y.CreatedAt).First().CreatedAt)
            .Select(x => x.Room)
            .ToArrayAsync();

        return items;
    }
}