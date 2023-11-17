using Microsoft.AspNetCore.SignalR;
using Theater.Abstractions.Caches;
using Theater.Consumer;
using Theater.Contracts.Rabbit;
using Theater.SignalR.Hubs;

namespace Theater.SignalR.Consumers;

public sealed class UserExitMessageConsumer : IMessageConsumer<UserExitModel>
{
    private readonly ILogger<UserExitMessageConsumer> _logger;
    private readonly IHubContext<ChatHub, IChatClient> _hubContext;
    private readonly IConnectionsCache _connectionsCache;

    public UserExitMessageConsumer(
        ILogger<UserExitMessageConsumer> logger, 
        IHubContext<ChatHub, IChatClient> hubContext, 
        IConnectionsCache connectionsCache)
    {
        _logger = logger;
        _hubContext = hubContext;
        _connectionsCache = connectionsCache;
    }

    public async Task ProcessMessage(UserExitModel message)
    {
        var connections = await _connectionsCache.GetConnections(message.UserId);
        if (connections is not { Count: not 0 })
            return;

        await _hubContext.Clients
            .Clients(connections)
            .OnRoomExit(message.RoomId);

        _logger.LogInformation("Пользователь {UserId} покинул в чат", message.UserId);
    }
}