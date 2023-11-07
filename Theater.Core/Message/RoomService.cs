using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using Theater.Abstractions;
using Theater.Abstractions.Rooms;
using Theater.Common.Enums;
using Theater.Contracts.Messages;
using Theater.Entities.Rooms;

namespace Theater.Core.Message;

public sealed class RoomService : BaseCrudService<RoomParameters, RoomEntity>, IRoomService
{
    public RoomService(
        IMapper mapper,
        IRoomRepository repository, 
        IDocumentValidator<RoomParameters> documentValidator,
        ILogger<BaseCrudService<RoomParameters, RoomEntity>> logger
        ) : base(mapper, repository, documentValidator, logger)
    {
    }

    protected override Task<RoomEntity> EnrichEntity(Guid? userId, RoomEntity entity)
    {
        if (userId.HasValue && entity.Users.All(x => x.UserId != userId))
            entity.Users.Add(new UserRoomEntity { IsActive = true, Role = RoomRole.Owner, UserId = userId.Value });

        return base.EnrichEntity(userId, entity);
    }
}