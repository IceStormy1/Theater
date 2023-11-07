using AutoMapper;
using MassTransit;
using System;
using System.Threading.Tasks;
using Theater.Abstractions.Errors;
using Theater.Abstractions.Rooms;
using Theater.Abstractions.UserAccount;
using Theater.Common;
using Theater.Common.Enums;
using Theater.Contracts.Messages;
using Theater.Contracts.Rabbit;
using Theater.Entities.Rooms;

namespace Theater.Core.Message;

public sealed class MessageService : IMessageService
{
    private readonly IRoomRepository _roomRepository;
    private readonly IUserAccountRepository _userAccountRepository;
    private readonly IMessageRepository _messageRepository;
    private readonly IPublishEndpoint _messageBus;
    private readonly IMapper _mapper;

    public MessageService(
        IRoomRepository roomRepository, 
        IUserAccountRepository userAccountRepository, 
        IMessageRepository messageRepository, 
        IPublishEndpoint messageBus, 
        IMapper mapper)
    {
        _roomRepository = roomRepository;
        _userAccountRepository = userAccountRepository;
        _messageRepository = messageRepository;
        _messageBus = messageBus;
        _mapper = mapper;
    }

    public async Task<Result<MessageModel>> SendMessage(Guid roomId, Guid userId, NewMessageModel newMessage)
    {
        var user = await _userAccountRepository.GetByEntityId(userId);

        if (user is null)
            return Result<MessageModel>.FromError(UserAccountErrors.NotFound.Error);

        var userRoom = await _roomRepository.GetActiveRoomForUser(userId, roomId);

        if (userRoom is null)
            return Result<MessageModel>.FromError(AbstractionErrors.NotFoundError.Error);

        var message = new MessageEntity
        {
            MessageType = MessageType.Text,
            Room = userRoom,
            Text = newMessage.Text,
            User = user
        };

        await _messageRepository.Add(message);

        var messageSentModel = new MessageSentModel
        {
            Id = message.Id,
            AuthorId = userId,
            CreatedAt = message.CreatedAt,
            MessageType = message.MessageType,
            RoomId = roomId,
            Text = newMessage.Text
        };

        await _messageBus.Publish(messageSentModel);

        var messageModel = _mapper.Map<MessageModel>(message);

        return Result.FromValue(messageModel);
    }
}