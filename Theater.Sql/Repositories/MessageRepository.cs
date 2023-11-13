using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Theater.Abstractions.Filters;
using Theater.Abstractions.Message;
using Theater.Entities.Rooms;

namespace Theater.Sql.Repositories;

public sealed class MessageRepository : BaseCrudRepository<MessageEntity>, IMessageRepository
{
    public MessageRepository(
        TheaterDbContext dbContext,
        ILogger<BaseCrudRepository<MessageEntity>> logger) : base(dbContext, logger)
    {
    }

    public async Task<List<MessageEntity>> GetMessages(Guid roomId, MessageFilterSettings filter)
    {
        return await DbSet.AsNoTracking()
            .Include(x => x.User)
            .Where(x => x.Room.Id == roomId)
            .OrderByDescending(x => x.CreatedAt)
            .Skip(filter.Offset)
            .Take(filter.Limit)
            .ToListAsync();
    }

    public async Task<IDictionary<Guid, MessageEntity>> GetLastMessagesForRooms(IEnumerable<Guid> roomIds)
    {
        var query = DbContext.Messages.AsNoTracking()
            .Include(x => x.Room)
            .Include(x => x.User)
            .Where(message => roomIds.Contains(message.Room.Id))
            .GroupBy(message => new { message.Room.Id })
            .Select(g => g.OrderByDescending(x => x.CreatedAt).First());

        return await query.ToDictionaryAsync(x => x.Room.Id, m => m);
    }

    public Task<DateTime?> GetLatestReadMessageTimeByRoomId(Guid roomId, Guid userId)
        => DbContext.UserRooms
            .Where(x => x.RoomId == roomId && x.IsActive && x.UserId != userId)
            .MaxAsync(x => x.LastReadMessageTime);
}