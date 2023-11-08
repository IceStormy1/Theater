using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Theater.Abstractions;
using Theater.Abstractions.Filters;
using Theater.Abstractions.Message;
using Theater.Abstractions.Rooms;
using Theater.Abstractions.UserAccount;
using Theater.Common;
using Theater.Common.Constants;
using Theater.Common.Enums;
using Theater.Contracts.Filters;
using Theater.Contracts.Messages;
using Theater.Contracts.Rooms;
using Theater.Entities.Rooms;

namespace Theater.Core.Rooms;

public sealed class RoomService : BaseCrudService<RoomParameters, RoomEntity>, IRoomService
{
    private readonly IUserAccountRepository _userAccountRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly ILogger<RoomService> _logger;
    private readonly IMessageRepository _messageRepository;

    public RoomService(
        IMapper mapper,
        IRoomRepository repository,
        IDocumentValidator<RoomParameters> documentValidator,
        ILogger<RoomService> logger, 
        IUserAccountRepository userAccountRepository,
        IRoomRepository roomRepository,
        IMessageRepository messageRepository) : base(mapper, repository, documentValidator, logger)
    {
        _logger = logger;
        _userAccountRepository = userAccountRepository;
        _roomRepository = roomRepository;
        _messageRepository = messageRepository;
    }

    protected override async Task<RoomEntity> EnrichEntity(Guid? userId, RoomEntity entity)
    {
        if (userId.HasValue && entity.Users.All(x => x.UserId != userId))
            entity.Users.Add(new UserRoomEntity { IsActive = true, Role = RoomRole.Owner, UserId = userId.Value });

        if(!entity.UpdatedAt.HasValue && entity.Type == RoomType.Group)
        {
            var systemUser = await _userAccountRepository.GetSystemUser();

            if (!systemUser.IsSuccess)
            {
                _logger.LogWarning("{ServiceName}: Ошибка при поиске системного пользователя", nameof(RoomService));

                return entity;
            }

            entity.Messages.Add(new MessageEntity
            {
                MessageType = MessageType.SystemText,
                Text = MessageConstants.SystemCreateRoom,
                User = systemUser.ResultData
            });
        }

        return await base.EnrichEntity(userId, entity);
    }

    public async Task<Result<List<RoomItemDto>>> GetRoomsForUser(Guid userId, RoomSearchParameters filter)
    {
        var filterSettings = Mapper.Map<RoomSearchSettings>(filter);
        var userRooms = await _roomRepository.GetRoomsForUser(userId, filterSettings);

        if (userRooms.Length == default)
            return Result.FromValue(new List<RoomItemDto>());

        //у индивидуальных чатов нету тайтла, потому что он зависит от того кто просматривает его в данный момент
        var individualRoomsIds = userRooms.Where(x => x.Type == RoomType.Individual)
            .Select(x => x.Id)
            .ToList();

        var users = await _roomRepository.GetUsersByIndividualRooms(userId, individualRoomsIds);

        var roomIds = userRooms.Select(x => x.Id).ToArray();
    
        var messages = await _messageRepository.GetLastMessagesForRooms(roomIds);

        var dtoItems = new List<RoomItemDto>();
        foreach (var room in userRooms)
        {
            var roomDto = Mapper.Map<RoomItemDto>(room);

            if (room.Type == RoomType.Individual && users.TryGetValue(roomDto.Id, out var user))
                roomDto.Title = user.FullName;

            if (messages.TryGetValue(roomDto.Id, out var message))
            {
                roomDto.LastMessage = Mapper.Map<LastMessageModel>(message);

                if (message.User.Id == userId)
                    roomDto.LastMessage.Status = MessageStatus.Seen;
            }

            dtoItems.Add(roomDto);
        }

        return Result.FromValue(dtoItems);
    }
}