using AutoMapper;
using Theater.Common.Extensions;
using Theater.Contracts.Theater.WorkersPosition;
using Theater.Entities.Theater;

namespace Theater.Core.Profiles;

public sealed class WorkersPositionProfile : Profile
{
    public WorkersPositionProfile()
    {
        CreateMap<WorkersPositionParameters, WorkersPositionEntity>();
        CreateMap<WorkersPositionEntity, WorkersPositionModel>()
            .ForMember(destination => destination.PositionTypeName, options => options.MapFrom(exp => exp.PositionType.GetEnumDisplayName()))
            ;
    }
}