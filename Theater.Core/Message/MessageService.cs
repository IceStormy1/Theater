using AutoMapper;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
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
using Theater.Entities.Users;

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
        var userRoom = await _roomRepository.GetActiveRoomRelationForUser(userId, roomId);

        if (userRoom is null)
            return Result<List<MessageModel>>.FromError(RoomErrors.RoomNotFoundError.Error);

        var filterSettings = _mapper.Map<MessageFilterSettings>(filter);

        var messages = await _messageRepository.GetMessages(roomId, filterSettings);

        var result = new List<MessageModel>();

        if (messages.Count == default)
            return Result.FromValue(result);

        var usersIds = messages.Select(x => x.UserId)
            .Distinct()
            .ToArray();

        var users = await _userAccountRepository.GetByEntityIds(usersIds);
        var usersDict = users.ToDictionary(x => x.Id, u => _mapper.Map<UserEntity, AuthorDto>(u));

        var lastReadMessageTime = await _messageRepository.GetLatestReadMessageTimeByRoomId(roomId, userId);

        foreach (var message in messages)
        {
            var messageDto = _mapper.Map<MessageModel>(message);

            messageDto.Author = usersDict[message.UserId];

            if (message.UserId == userId) //проверяем прочитано ли сообщение другими пользователями
            {
                messageDto.Status = lastReadMessageTime >= message.CreatedAt
                    ? MessageStatus.Seen
                    : MessageStatus.Unseen;
            }
            else
            {
                messageDto.Status = userRoom.LastReadMessageTime >= message.CreatedAt
                    ? MessageStatus.Seen
                    : MessageStatus.Unseen;
            }

            result.Add(messageDto);
        }

        return Result.FromValue(result);
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