using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using Theater.Abstractions;
using Theater.Abstractions.Authorization.Models;
using Theater.Abstractions.Filters;
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
using Theater.Contracts.Theater.PurchasedUserTicket;
using Theater.Contracts.Theater.TheaterWorker;
using Theater.Contracts.Theater.UserReview;
using Theater.Contracts.Theater.WorkersPosition;
using Theater.Contracts.UserAccount;
using Theater.Entities.Authorization;
using Theater.Entities.FileStorage;
using Theater.Entities.Theater;
using VkNet.Model.RequestParams;
using GenderType = Theater.Entities.Authorization.GenderType;

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

        CreateMap<AccountSaveProfileInfoParams, UserEntity>()
            .ForMember(destination => destination.Id, options => options.Ignore())
            .ForMember(destination => destination.DateOfCreate, options => options.MapFrom(_ => DateTime.UtcNow))
            .ForMember(destination => destination.RoleId, options => options.MapFrom(_ => (int)UserRole.User))
            .ForMember(destination => destination.Money, options => options.MapFrom(_ => (decimal)default))
            .ForMember(destination => destination.VkId, options => options.MapFrom(exp => exp.Id))
            .ForMember(destination => destination.UserName, options => options.MapFrom(exp => exp.ScreenName))
            .ForMember(destination => destination.FirstName, options => options.MapFrom(exp => exp.FirstName))
            .ForMember(destination => destination.LastName, options => options.MapFrom(exp => exp.LastName))
            .ForMember(destination => destination.Gender, options => options.MapFrom(exp => (int)exp.Sex == 1 ? GenderType.Female : GenderType.Male))
            .ForMember(destination => destination.Email, options => options.MapFrom(exp => "test@mail.ru")) // TODO: разобраться откуда брать инфу о почте и телефоне в нормальном виде
            .ForMember(destination => destination.Phone, options => options.MapFrom(exp => "79096478398")) // TODO: разобраться откуда брать инфу о почте и телефоне в нормальном виде
            .ForMember(destination => destination.BirthDate, options => options.MapFrom(exp => exp.BirthDate))
            ;

        CreateMap<UserEntity, UserModel>()
            .ForMember(destination => destination.Password, options => options.MapFrom(exp => exp.Password))
            .ForMember(destination => destination.Photo, options => options.MapFrom(exp => exp.PhotoId.HasValue ? new StorageFileListItem{Id = exp.PhotoId.Value} : null))
            ;

        CreateMap<UserEntity, AuthenticateResponse>();
        CreateMap<PurchasedUserTicketEntity, PurchasedUserTicketModel>()
            .ForMember(destination => destination.PieceDate, options => options.MapFrom(exp => exp.TicketPriceEvents.PiecesTicket.PieceDate.Date))
            .ForMember(destination => destination.PieceDateId, options => options.MapFrom(exp => exp.TicketPriceEvents.PiecesTicket.PieceDateId))
            .ForMember(destination => destination.PieceName, options => options.MapFrom(exp => exp.TicketPriceEvents.PiecesTicket.PieceDate.Piece.PieceName))
            .ForMember(destination => destination.PieceId, options => options.MapFrom(exp => exp.TicketPriceEvents.PiecesTicket.PieceDate.PieceId))
            .ForMember(destination => destination.TicketRow, options => options.MapFrom(exp => exp.TicketPriceEvents.Model.TicketRow))
            .ForMember(destination => destination.TicketPlace, options => options.MapFrom(exp => exp.TicketPriceEvents.Model.TicketPlace))
            .ForMember(destination => destination.TicketPrice, options => options.MapFrom(exp => exp.TicketPriceEvents.Model.TicketPrice))
            ;

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
            .ForMember(destination => destination.MainPhoto, options => options.MapFrom(exp => exp.PhotoId.HasValue ? new StorageFileListItem { Id = exp.PhotoId.Value } : null))
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
            .ForMember(destination => destination.WorkerShortInformation, options => options.MapFrom(exp => exp.PieceWorkers.Select(x => x.TheaterWorker).ToList()))
            ;

        CreateMap<PieceEntity, PieceBase>();

        CreateMap<TheaterWorkerEntity, TheaterWorkerModel>()
            .ForMember(destination => destination.PositionTypeName, options => options.MapFrom(exp => exp.Position.PositionType.GetEnumDisplayName()))
            .ForMember(destination => destination.PositionName, options => options.MapFrom(exp => exp.Position.PositionName))
            .ForMember(destination => destination.PositionType, options => options.MapFrom(exp => exp.Position.PositionType))
            .ForMember(destination => destination.BirthDate, options => options.MapFrom(exp => exp.DateOfBirth))
            .ForMember(destination => destination.Pieces, options => options.MapFrom(exp => exp.PieceWorkers.Select(x=>x.Piece)))
            .ForMember(destination => destination.MainPhoto, options => options.MapFrom(exp => exp.PhotoId.HasValue ? new StorageFileListItem { Id = exp.PhotoId.Value } : null))
            ;

        CreateMap<WriteResult<TheaterWorkerEntity>, WriteResult<TheaterWorkerModel>>();
        CreateMap<TheaterWorkerParameters, TheaterWorkerEntity>()
            .ForMember(destination => destination.DateOfBirth, options => options.MapFrom(exp => exp.BirthDate))
            .ForMember(destination => destination.PhotoId, options => options.MapFrom(exp => exp.MainPhoto.Id))
            ;

        CreateMap<PiecesTicketEntity, PiecesTicketModel>()
            .ForMember(destination => destination.IsBooked, options => options.MapFrom(exp => exp.BookedTicket != null || exp.TicketPriceEvents.Any(c=>c.PurchasedUserTicket != null)))
            ;

        CreateMap<PieceDateParameters, PieceDateEntity>();
        CreateMap<PiecesGenreEntity, PiecesGenreModel>();
           
        CreateMap<PieceDateEntity, PieceDateModel>();

        CreateMap<PiecesTicketParameters, PiecesTicketEntity>();
        CreateMap<PiecesTicketModel, PiecesTicketEntity>();

        CreateMap<PieceFilterParameters, PieceFilterSettings>();
        CreateMap<TheaterWorkerFilterParameters, TheaterWorkerFilterSettings>();
        CreateMap<UserAccountFilterParameters, UserAccountFilterSettings>();
        CreateMap<PieceTicketFilterParameters, PieceTicketFilterSettings>();
        CreateMap<UserReviewFilterParameters, UserReviewFilterSettings>();

        CreateMap<UserReviewParameters, UserReviewEntity>();
        CreateMap<UserReviewEntity, UserReviewModel>()
            .ForMember(destination => destination.UserName, options => options.MapFrom(exp => exp.User == null ? null : exp.User.UserName))
            .ForMember(destination => destination.PieceName, options => options.MapFrom(exp => exp.User == null ? null : exp.Piece.PieceName))
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