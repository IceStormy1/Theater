using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Theater.Abstractions.Caches;
using Theater.Common.Enums;
using Theater.Consumer;
using Theater.Contracts.Messages;
using Theater.Contracts.Rabbit;
using Theater.SignalR.Hubs;

namespace Theater.SignalR.Consumers;

public sealed class MessageSentConsumer : IMessageConsumer<MessageSentModel>
{
    private readonly ILogger<MessageSentConsumer> _logger;
    private readonly IHubContext<ChatHub, IChatClient> _hubContext;
    private readonly IMapper _mapper;
    private readonly IConnectionsCache _connectionsCache;

    public MessageSentConsumer(
        ILogger<MessageSentConsumer> logger,
        IHubContext<ChatHub, IChatClient> hubContext,
        IMapper mapper,
        IConnectionsCache connectionsCache)
    {
        _logger = logger;
        _hubContext = hubContext;
        _mapper = mapper;
        _connectionsCache = connectionsCache;
    }

    public async Task ProcessMessage(MessageSentModel message)
    {
        IChatClient group;
        if (message.MessageType != MessageType.SystemText)
        {
            var authorConnections = await _connectionsCache.GetConnections(message.AuthorId);
            group = _hubContext.Clients.GroupExcept(
                    groupName: message.RoomId.ToString(), 
                    excludedConnectionIds: authorConnections);
        }
        else
        {
            group = _hubContext.Clients.Group(message.RoomId.ToString());
        }

        var messageDto = _mapper.Map<MessageModel>(message);

        await group.OnMessageReceived(message.RoomId, messageDto);
        await _hubContext.Clients
            .User(message.AuthorId.ToString())
            .OnMessageDelivered(message.RoomId, messageDto);

        _logger.LogInformation("Сообщение {messageId} для чата {roomId} отправлено", message.Id, message.RoomId);
    }
}