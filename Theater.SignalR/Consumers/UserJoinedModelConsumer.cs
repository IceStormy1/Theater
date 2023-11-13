using Microsoft.AspNetCore.SignalR;
using Theater.Consumer;
using Theater.Contracts.Rabbit;
using Theater.SignalR.Hubs;

namespace Theater.SignalR.Consumers;

public sealed class UserJoinedModelConsumer : IMessageConsumer<UserJoinedModel>
{
    private readonly ILogger<UserJoinedModelConsumer> _logger;
    private readonly IHubContext<ChatHub, IChatClient> _hubContext;
    private readonly ChatManager _chatManager;

    public UserJoinedModelConsumer(
        ILogger<UserJoinedModelConsumer> logger, 
        IHubContext<ChatHub, IChatClient> hubContext, 
        ChatManager chatManager)
    {
        _logger = logger;
        _hubContext = hubContext;
        _chatManager = chatManager;
    }

    public async Task ProcessMessage(UserJoinedModel message)
    {
        var connections = _chatManager.GetUserConnectionsById(message.UserId);
        if (connections is not { Count: not 0 })
            return;

        await _hubContext.Clients
            .Clients(connections.Select(x => x.ConnectionId))
            .OnRoomEnter(message.RoomId, message.RoomTitle);

        _logger.LogInformation("Пользователь {UserId} добавлен в чат", message.UserId);
    }
}