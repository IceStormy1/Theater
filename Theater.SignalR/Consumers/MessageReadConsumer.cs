using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Theater.Consumer;
using Theater.Contracts.Rabbit;
using Theater.Contracts.Rooms;
using Theater.SignalR.Hubs;

namespace Theater.SignalR.Consumers;

public sealed class MessageReadConsumer : IMessageConsumer<MessageReadModel>
{
    private readonly IHubContext<ChatHub, IChatClient> _hubContext;
    private readonly IMapper _mapper;
    private readonly ILogger<MessageReadConsumer> _logger;
    private readonly ChatManager _chatManager;

    public MessageReadConsumer(
        IHubContext<ChatHub, IChatClient> hubContext, 
        IMapper mapper, 
        ILogger<MessageReadConsumer> logger, 
        ChatManager chatManager)
    {
        _hubContext = hubContext;
        _mapper = mapper;
        _logger = logger;
        _chatManager = chatManager;
    }

    public async Task ProcessMessage(MessageReadModel message)
    {
        var connections = _chatManager.GetUserConnectionsById(message.MessageAuthorId);
        if (connections is not { Count: not 0 })
            return;

        await _hubContext.Clients
            .Clients(connections.Select(x => x.ConnectionId))
            .OnMessageRead(_mapper.Map<ReadMessageModel>(message));

        _logger.LogInformation("Сообщение {messageId} для чата {roomId} прочитано", message.MessageId, message.RoomId);
    }
}