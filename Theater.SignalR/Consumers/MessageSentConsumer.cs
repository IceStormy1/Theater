using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Theater.Common.Enums;
using Theater.Consumer;
using Theater.Contracts.Messages;
using Theater.Contracts.Rabbit;
using Theater.SignalR.Hubs;

namespace Theater.SignalR.Consumers;

public class MessageSentConsumer : IMessageConsumer<MessageSentModel>
{
    private readonly ILogger<MessageSentConsumer> _logger;
    private readonly IHubContext<ChatHub, IChatClient> _hubContext;
    private readonly IMapper _mapper;

    public MessageSentConsumer(
        ILogger<MessageSentConsumer> logger,
        IHubContext<ChatHub, IChatClient> hubContext,
        IMapper mapper
        )
    {
        _logger = logger;
        _hubContext = hubContext;
        _mapper = mapper;
    }

    public async Task ProcessMessage(MessageSentModel message)
    {
        IChatClient group;

        if (message.MessageType != MessageType.SystemText)
        {
            group = _hubContext.Clients.GroupExcept(message.RoomId.ToString(), new List<string>());
            if (group == null)
            {
                return;
            }
        }
        else
        {
            group = _hubContext.Clients.Group(message.RoomId.ToString());
            if (group == null)
            {
                return;
            }
        }

        var messageDto = _mapper.Map<MessageModel>(message);

        await group.OnMessageReceived(message.RoomId, messageDto);
        await _hubContext.Clients
            .User(message.AuthorId.ToString())
            .OnMessageDelivered(message.RoomId, messageDto);

        _logger.LogInformation("Сообщение {messageId} для чата {roomId} отправлено", message.Id, message.RoomId);
    }
}