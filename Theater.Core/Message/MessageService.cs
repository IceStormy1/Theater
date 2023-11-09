using AutoMapper;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Theater.Abstractions.Errors;
using Theater.Abstractions.Filters;
using Theater.Abstractions.Message;
using Theater.Abstractions.Rooms;
using Theater.Abstractions.UserAccount;
using Theater.Common;
using Theater.Common.Enums;
using Theater.Contracts.Filters;
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
        var userRoom = await _roomRepository.GetActiveRoomForUser(userId, roomId);

        if (userRoom is null)
            return Result<MessageModel>.FromError(RoomErrors.RoomNotFoundError.Error);

        var message = new MessageEntity
        {
            MessageType = MessageType.Text,
            RoomId = roomId,
            Text = newMessage.Text,
            UserId = userId
        };

        await _messageRepository.Add(message);

        await PublishMessageSent(userId, roomId, message);

        var messageModel = _mapper.Map<MessageModel>(message);

        return Result.FromValue(messageModel);
    }

    public async Task<Result<List<MessageModel>>> GetMessages(Guid roomId, Guid userId, MessageFilterParameters filter)
    {
        var isMemberOfRoom = await _roomRepository.IsMemberOfRoom(userId, roomId);

        if (!isMemberOfRoom)
            return Result<List<MessageModel>>.FromError(RoomErrors.RoomNotFoundError.Error);

        var filterSettings = _mapper.Map<MessageFilterSettings>(filter);

        var messages = await _messageRepository.GetMessages(roomId, filterSettings);

        return Result.FromValue(_mapper.Map<List<MessageModel>>(messages));
    }

    public async Task PublishMessageSent(Guid userId, Guid roomId, MessageEntity message)
    {
        var messageSentModel = new MessageSentModel
        {
            Id = message.Id,
            AuthorId = userId,
            CreatedAt = message.CreatedAt,
            MessageType = message.MessageType,
            RoomId = roomId,
            Text = message.Text
        };

        await _messageBus.Publish(messageSentModel);
    }
}