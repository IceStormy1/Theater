using AutoMapper;
using System.Linq;
using Theater.Abstractions.Filters;
using Theater.Abstractions.Piece.Models;
using Theater.Common;
using Theater.Common.Extensions;
using Theater.Contracts.FileStorage;
using Theater.Contracts.Filters;
using Theater.Contracts.Theater.TheaterWorker;
using Theater.Entities.Theater;

namespace Theater.Core.Profiles;

public sealed class TheaterWorkerProfile : Profile
{
    public TheaterWorkerProfile()
    {
        CreateMap<TheaterWorkerEntity, TheaterWorkerShortInformationModel>()
            .ForMember(destination => destination.PositionTypeName, options => options.MapFrom(exp => exp.Position.PositionType.GetEnumDisplayName()))
            .ForMember(destination => destination.PositionType, options => options.MapFrom(exp => exp.Position.PositionType))
            .ForMember(destination => destination.PositionName, options => options.MapFrom(exp => exp.Position.PositionName))
            .ForMember(destination => destination.FullName, options => options.MapFrom(exp => $"{exp.LastName} {exp.FirstName} {exp.MiddleName}"))
            .ForMember(destination => destination.MainPhoto, options => options.MapFrom(exp => exp.PhotoId.HasValue ? new StorageFileListItem { Id = exp.PhotoId.Value } : null))
            ;

        CreateMap<TheaterWorkerEntity, TheaterWorkerModel>()
            .ForMember(destination => destination.PositionTypeName, options => options.MapFrom(exp => exp.Position.PositionType.GetEnumDisplayName()))
            .ForMember(destination => destination.PositionName, options => options.MapFrom(exp => exp.Position.PositionName))
            .ForMember(destination => destination.PositionType, options => options.MapFrom(exp => exp.Position.PositionType))
            .ForMember(destination => destination.BirthDate, options => options.MapFrom(exp => exp.DateOfBirth))
            .ForMember(destination => destination.Pieces, options => options.MapFrom(exp => exp.PieceWorkers.Select(x => x.Piece)))
            .ForMember(destination => destination.MainPhoto, options => options.MapFrom(exp => exp.PhotoId.HasValue ? new StorageFileListItem { Id = exp.PhotoId.Value } : null))
            ;

        CreateMap<Result<TheaterWorkerEntity>, Result<TheaterWorkerModel>>();
        CreateMap<TheaterWorkerParameters, TheaterWorkerEntity>()
            .ForMember(destination => destination.DateOfBirth, options => options.MapFrom(exp => exp.BirthDate))
            .ForMember(destination => destination.PhotoId, options => options.MapFrom(exp => exp.MainPhoto.Id))
            ;

        CreateMap<TheaterWorkerShortInformationDto, TheaterWorkerShortInformationModel>()
            .ForMember(destination => destination.PositionTypeName, options => options.MapFrom(exp => exp.PositionTypeName.GetEnumDisplayName()))
            ;

        CreateMap<TheaterWorkerFilterParameters, TheaterWorkerFilterSettings>();
    }
}