using AutoMapper;
using Theater.Contracts.Messages;
using Theater.Entities.Rooms;

namespace Theater.Core.Profiles;

public sealed class RoomProfile : Profile
{
    public RoomProfile()
    {
        CreateMap<RoomParameters, RoomEntity>();
    }
}