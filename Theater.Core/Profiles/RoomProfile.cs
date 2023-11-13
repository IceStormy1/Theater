using System.Linq;
using AutoMapper;
using Theater.Abstractions.Filters;
using Theater.Contracts.Filters;
using Theater.Contracts.Rooms;
using Theater.Entities.Rooms;

namespace Theater.Core.Profiles;

public sealed class RoomProfile : Profile
{
    public RoomProfile()
    {
        CreateMap<RoomParameters, RoomEntity>();
        CreateMap<RoomSearchParameters, RoomSearchSettings>();
        CreateMap<RoomEntity, RoomItemDto>()
            .ForMember(destination => destination.Users, options => options.MapFrom(exp => exp.Users.Select(c=>c.User)))
            ;
    }
}