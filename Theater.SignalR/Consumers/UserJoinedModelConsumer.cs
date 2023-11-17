using Microsoft.AspNetCore.SignalR;
using Theater.Abstractions.Caches;
using Theater.Consumer;
using Theater.Contracts.Rabbit;
using Theater.SignalR.Hubs;

namespace Theater.SignalR.Consumers;

public sealed class UserJoinedModelConsumer : IMessageConsumer<UserJoinedModel>
{
    private readonly ILogger<UserJoinedModelConsumer> _logger;
    private readonly IHubContext<ChatHub, IChatClient> _hubContext;
    private readonly IConnectionsCache _connectionsCache;

    public UserJoinedModelConsumer(
        ILogger<UserJoinedModelConsumer> logger, 
        IHubContext<ChatHub, IChatClient> hubContext, 
        IConnectionsCache connectionsCache)
    {
        _logger = logger;
        _hubContext = hubContext;
        _connectionsCache = connectionsCache;
    }

    public async Task ProcessMessage(UserJoinedModel message)
    {
        var connections = await _connectionsCache.GetConnections(message.UserId);
        if (connections is not { Count: not 0 })
            return;

        await _hubContext.Clients
            .Clients(connections)
            .OnRoomEnter(message.RoomId, message.RoomTitle);

        _logger.LogInformation("Пользователь {UserId} добавлен в чат", message.UserId);
    }
}