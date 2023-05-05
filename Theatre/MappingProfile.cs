using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using Theater.Abstractions;
using Theater.Abstractions.Authorization.Models;
using Theater.Abstractions.Filter;
using Theater.Abstractions.Piece.Models;
using Theater.Common;
using Theater.Common.Extensions;
using Theater.Contracts;
using Theater.Contracts.Authorization;
using Theater.Contracts.FileStorage;
using Theater.Contracts.Filters;
using Theater.Contracts.Theater.Piece;
using Theater.Contracts.Theater.PieceDate;
using Theater.Contracts.Theater.PiecesGenre;
using Theater.Contracts.Theater.PiecesTicket;
using Theater.Contracts.Theater.PieceWorker;
using Theater.Contracts.Theater.TheaterWorker;
using Theater.Contracts.Theater.UserReview;
using Theater.Contracts.Theater.WorkersPosition;
using Theater.Contracts.UserAccount;
using Theater.Entities.Authorization;
using Theater.Entities.FileStorage;
using Theater.Entities.Theater;

namespace Theater;

internal sealed class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<UserParameters, UserEntity>()
            .ForMember(destination => destination.DateOfCreate, options => options.MapFrom(_ => DateTime.UtcNow))
            .ForMember(destination => destination.Money, options => options.MapFrom(_ => (decimal)default))
            .ForMember(destination => destination.RoleId, options => options.MapFrom(_ => (int)UserRole.User))
            .ForMember(destination => destination.PhotoId, options => options.MapFrom(x => x.Photo == null ? (Guid?)null : x.Photo.Id))
            .ForMember(destination => destination.Photo, options => options.Ignore())
            ;

        CreateMap<UserEntity, UserModel>()
            .ForMember(destination => destination.Password, options => options.MapFrom(exp => exp.Password))
            .ForMember(destination => destination.Photo, options => options.MapFrom(exp => exp.PhotoId.HasValue ? new StorageFileListItem{Id = exp.PhotoId.Value} : null))
            ;

        CreateMap<UserEntity, AuthenticateResponse>();

        CreateMap<PieceEntity, PieceShortInformationModel>()
            .ForMember(destination => destination.MainPicture, options => options.MapFrom(exp => new StorageFileListItem { Id = exp.MainPhotoId }))
            .ForMember(destination => destination.WorkerShortInformation, options => options.MapFrom(exp => exp.PieceWorkers.Select(x=>x.TheaterWorker).ToList()))
            .ForMember(destination => destination.PieceGenre, options => options.MapFrom(exp => exp.Genre.GenreName))
            ;

        CreateMap<PieceDateDto, PieceDateModel>();
        CreateMap<TheaterWorkerShortInformationDto, TheaterWorkerShortInformationModel>()
            .ForMember(destination => destination.PositionTypeName, options => options.MapFrom(exp => exp.PositionTypeName.GetEnumDisplayName()))
            ;

        CreateMap<TheaterWorkerEntity, TheaterWorkerShortInformationModel>()
            .ForMember(destination => destination.PositionTypeName, options => options.MapFrom(exp => exp.Position.PositionType.GetEnumDisplayName()))
            .ForMember(destination => destination.PositionType, options => options.MapFrom(exp => exp.Position.PositionType))
            .ForMember(destination => destination.PositionName, options => options.MapFrom(exp => exp.Position.PositionName))
            .ForMember(destination => destination.FullName, options => options.MapFrom(exp => $"{exp.LastName} {exp.FirstName} {exp.MiddleName}"))
            ;

        CreateMap<PieceDto, PieceModel>();
        CreateMap<PieceParameters, PieceEntity>()
            .ForMember(destination => destination.MainPhotoId, options => options.MapFrom(exp => exp.MainPhoto.Id))
            .ForMember(destination => destination.PhotoIds, options => options.MapFrom(exp => exp.AdditionalPhotos != null ? exp.AdditionalPhotos.Select(x=>x.Id) : new List<Guid>()))
            .ForMember(destination => destination.MainPhoto, options => options.Ignore())
            ;

        CreateMap<PieceEntity, PieceModel>()
            .ForMember(destination => destination.MainPicture, options => options.MapFrom(exp => new StorageFileListItem { Id = exp.MainPhotoId }))
            .ForMember(destination => destination.AdditionalPhotos, options => options.MapFrom(exp => exp.PhotoIds.Select(x => new StorageFileListItem { Id = x }).ToList()))
            .ForMember(destination => destination.PieceGenre, options => options.MapFrom(exp => exp.Genre.GenreName))

            ;

        CreateMap<TheaterWorkerEntity, TheaterWorkerModel>()
            .ForMember(destination => destination.PositionTypeName, options => options.MapFrom(exp => exp.Position.PositionType.GetEnumDisplayName()))
            .ForMember(destination => destination.PositionName, options => options.MapFrom(exp => exp.Position.PositionName))
            .ForMember(destination => destination.PositionType, options => options.MapFrom(exp => exp.Position.PositionType))
            .ForMember(destination => destination.BirthDate, options => options.MapFrom(exp => exp.DateOfBirth))
            ;

        CreateMap<WriteResult<TheaterWorkerEntity>, WriteResult<TheaterWorkerModel>>();
        CreateMap<TheaterWorkerParameters, TheaterWorkerEntity>()
            .ForMember(destination => destination.DateOfBirth, options => options.MapFrom(exp => exp.BirthDate))
            ;
            
        CreateMap<PiecesTicketEntity, PiecesTicketModel>()
            .ForMember(destination => destination.IsBooked, options => options.MapFrom(exp => exp.BookedTicket == null))
            ;

        CreateMap<PieceDateParameters, PieceDateEntity>();
           
        CreateMap<PieceDateEntity, PieceDateModel>();

        CreateMap<PiecesTicketEntity, PiecesTicketModel>();
        CreateMap<PiecesTicketParameters, PiecesTicketEntity>();
        CreateMap<PiecesTicketModel, PiecesTicketEntity>();

        CreateMap<PieceFilterParameters, PieceFilterSettings>();
        CreateMap<TheaterWorkerFilterParameters, TheaterWorkerFilterSettings>();
        CreateMap<UserAccountFilterParameters, UserAccountFilterSettings>();

        CreateMap<UserReviewParameters, UserReviewEntity>();
        CreateMap<UserReviewEntity, UserReviewModel>()
            .ForMember(destination => destination.UserName, options => options.MapFrom(exp => exp.User == null ? null : exp.User.UserName))
            ;

        CreateMap<PieceWorkerParameters, PieceWorkerEntity>();
        CreateMap<PiecesGenreParameters, PiecesGenreEntity>();

        CreateMap<FileStorageEntity, StorageFileListItem>();

        CreateMap<WorkersPositionParameters, WorkersPositionEntity>();
        CreateMap<UserEntity, UserShortItem>();
        CreateMap<WorkersPositionEntity, WorkersPositionModel>()
            .ForMember(destination => destination.PositionTypeName, options => options.MapFrom(exp => exp.PositionType.GetEnumDisplayName()))
            ;

        CreateMap(typeof(PagingResult<>), typeof(Page<>));
    }
}