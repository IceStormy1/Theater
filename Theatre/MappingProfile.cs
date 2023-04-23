using AutoMapper;
using System;
using Theater.Abstractions;
using Theater.Abstractions.Authorization.Models;
using Theater.Abstractions.Filter;
using Theater.Abstractions.Piece.Models;
using Theater.Common;
using Theater.Common.Extensions;
using Theater.Contracts;
using Theater.Contracts.Authorization;
using Theater.Contracts.Filters;
using Theater.Contracts.Theater;
using Theater.Entities.Authorization;
using Theater.Entities.Theater;

namespace Theater
{
    internal sealed class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserParameters, UserEntity>()
                .ForMember(destination => destination.DateOfCreate, options => options.MapFrom(_ => DateTime.UtcNow))
                .ForMember(destination => destination.Money, options => options.MapFrom(_ => (decimal)default))
                .ForMember(destination => destination.RoleId, options => options.MapFrom(_ => (int)UserRole.User))
                ;

            CreateMap<UserEntity, UserModel>()
                .ForMember(destination => destination.Password, options => options.MapFrom(exp => exp.Password))
                ;

            CreateMap<UserEntity, AuthenticateResponse>();

            CreateMap<PieceShortInformationDto, PieceShortInformationModel>();
            CreateMap<PieceDateDto, PieceDateModel>();
            CreateMap<TheaterWorkerShortInformationDto, TheaterWorkerShortInformationModel>()
                .ForMember(destination => destination.PositionTypeName, options => options.MapFrom(exp => exp.PositionTypeName.GetEnumDisplayName()))
                ;
            CreateMap<TheaterWorkerEntity, TheaterWorkerShortInformationModel>()
                .ForMember(destination => destination.PositionTypeName, options => options.MapFrom(exp => exp.Position.PositionType.GetEnumDisplayName()))
                .ForMember(destination => destination.PositionName, options => options.MapFrom(exp => exp.Position.PositionName))
                .ForMember(destination => destination.FullName, options => options.MapFrom(exp => $"{exp.LastName} {exp.FirstName} {exp.MiddleName}"))
                ;

            CreateMap<PieceDto, PieceModel>();
            CreateMap<PieceParameters, PieceEntity>();
            CreateMap<PieceEntity, PieceModel>();

            CreateMap<TheaterWorkerEntity, TheaterWorkerModel>()
                .ForMember(destination => destination.PositionTypeName, options => options.MapFrom(exp => exp.Position.PositionType.GetEnumDisplayName()))
                .ForMember(destination => destination.PositionName, options => options.MapFrom(exp => exp.Position.PositionName))
                .ForMember(destination => destination.PositionType, options => options.MapFrom(exp => exp.Position.PositionType))
                ;
            CreateMap<WriteResult<TheaterWorkerEntity>, WriteResult<TheaterWorkerModel>>();
            CreateMap<TheaterWorkerParameters, TheaterWorkerEntity>()
                .ForMember(destination => destination.DateOfBirth, options => options.MapFrom(exp => exp.BirthDate))
                ;
            
            CreateMap<PiecesTicketEntity, PiecesTicketModel>();
            CreateMap<PieceDateParameters, PieceDateEntity>();
           
            CreateMap<PieceDateEntity, PieceDateModel>()
                .ForMember(destination => destination.PiecesTickets, options => options.MapFrom(exp => exp.PiecesTickets))
                ;

            CreateMap<PiecesTicketEntity, PiecesTicketModel>();
            CreateMap<PiecesTicketParameters, PiecesTicketEntity>();
            CreateMap<PiecesTicketModel, PiecesTicketEntity>();

            CreateMap<PieceFilterParameters, PieceFilterSettings>();
            CreateMap<TheaterWorkerFilterParameters, TheaterWorkerFilterSettings>();

            CreateMap<UserReviewParameters, UserReviewEntity>();
            CreateMap<UserReviewEntity, UserReviewModel>()
                .ForMember(destination => destination.UserName, options => options.MapFrom(exp => exp.User == null ? null : exp.User.UserName))
                ;

            CreateMap<PieceWorkerParameters, PieceWorkerEntity>();
            CreateMap<PiecesGenreParameters, PiecesGenreEntity>();
            
            CreateMap(typeof(PagingResult<>), typeof(Page<>));
        }
    }
}