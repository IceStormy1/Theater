using Microsoft.Extensions.Logging;
using Theater.Abstractions.Rooms;
using Theater.Entities.Rooms;

namespace Theater.Sql.Repositories;

public sealed class MessageRepository : BaseCrudRepository<MessageEntity>, IMessageRepository
{
    public MessageRepository(
        TheaterDbContext dbContext,
        ILogger<BaseCrudRepository<MessageEntity>> logger) : base(dbContext, logger)
    {
    }
}