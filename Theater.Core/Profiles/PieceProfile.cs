using System.Collections.Generic;
using System;
using System.Linq;
using AutoMapper;
using Theater.Contracts.FileStorage;
using Theater.Contracts.Theater.Piece;
using Theater.Entities.Theater;
using Theater.Abstractions.Piece.Models;
using Theater.Abstractions.Filters;
using Theater.Contracts.Filters;

namespace Theater.Core.Profiles;

public sealed class PieceProfile : Profile
{
    public PieceProfile()
    {
        CreateMap<PieceEntity, PieceShortInformationModel>()
            .ForMember(destination => destination.MainPicture, options => options.MapFrom(exp => new StorageFileListItem { Id = exp.MainPhotoId }))
            .ForMember(destination => destination.WorkerShortInformation, options => options.MapFrom(exp => exp.PieceWorkers.Select(x => x.TheaterWorker).ToList()))
            .ForMember(destination => destination.PieceGenre, options => options.MapFrom(exp => exp.Genre.GenreName))
            ;

        CreateMap<PieceParameters, PieceEntity>()
            .ForMember(destination => destination.MainPhotoId, options => options.MapFrom(exp => exp.MainPhoto.Id))
            .ForMember(destination => destination.PhotoIds, options => options.MapFrom(exp => exp.AdditionalPhotos != null ? exp.AdditionalPhotos.Select(x => x.Id) : new List<Guid>()))
            .ForMember(destination => destination.MainPhoto, options => options.Ignore())
            ;

        CreateMap<PieceEntity, PieceModel>()
            .ForMember(destination => destination.MainPicture, options => options.MapFrom(exp => new StorageFileListItem { Id = exp.MainPhotoId }))
            .ForMember(destination => destination.AdditionalPhotos, options => options.MapFrom(exp => exp.PhotoIds.Select(x => new StorageFileListItem { Id = x }).ToList()))
            .ForMember(destination => destination.PieceGenre, options => options.MapFrom(exp => exp.Genre.GenreName))
            .ForMember(destination => destination.WorkerShortInformation, options => options.MapFrom(exp => exp.PieceWorkers.Select(x => x.TheaterWorker).ToList()))
            ;

        CreateMap<PieceEntity, PieceBase>();
        CreateMap<PieceDto, PieceModel>();

        CreateMap<PieceFilterParameters, PieceFilterSettings>();
    }
}