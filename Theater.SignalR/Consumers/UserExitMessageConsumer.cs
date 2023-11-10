using Microsoft.AspNetCore.SignalR;
using Theater.Consumer;
using Theater.Contracts.Rabbit;
using Theater.SignalR.Hubs;

namespace Theater.SignalR.Consumers;

public sealed class UserExitMessageConsumer : IMessageConsumer<UserExitMessage>
{
    private readonly ILogger<UserExitMessageConsumer> _logger;
    private readonly IHubContext<ChatHub, IChatClient> _hubContext;
    private readonly ChatManager _chatManager;

    public UserExitMessageConsumer(
        ILogger<UserExitMessageConsumer> logger, 
        IHubContext<ChatHub, IChatClient> hubContext, 
        ChatManager chatManager)
    {
        _logger = logger;
        _hubContext = hubContext;
        _chatManager = chatManager;
    }

    public async Task ProcessMessage(UserExitMessage message)
    {
        var connections = _chatManager.GetUserConnectionsById(message.UserId);
        if (connections is not { Count: not 0 })
            return;

        await _hubContext.Clients
            .Clients(connections.Select(x => x.ConnectionId))
            .OnRoomExit(message.RoomId);

        _logger.LogInformation("Пользователь {UserId} покинул в чат", message.UserId);
    }
}