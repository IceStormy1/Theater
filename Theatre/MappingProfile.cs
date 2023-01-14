using AutoMapper;
using System;
using System.Collections.Generic;
using Theater.Abstractions.Authorization.Models;
using Theater.Abstractions.Piece.Models;
using Theater.Common;
using Theater.Common.Extensions;
using Theater.Contracts.Authorization;
using Theater.Contracts.Theater;
using Theater.Entities.Authorization;
using Theater.Entities.Theater;

namespace Theater
{
    internal class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserParameters, UserEntity>()
                .ForMember(destination => destination.DateOfCreate, options => options.MapFrom(_ => DateTime.UtcNow))
                .ForMember(destination => destination.Money, options => options.MapFrom(_ => (decimal)default))
                .ForMember(destination => destination.RoleId, options => options.MapFrom(_ => (int)UserRole.User));

            CreateMap<UserEntity, UserModel>()
                .ForMember(destination => destination.Password, options => options.MapFrom(c => c.Password));

            CreateMap<UserEntity, AuthenticateResponse>();

            CreateMap<PieceShortInformationDto, PieceShortInformationModel>();
            CreateMap<PieceDateDto, PieceDateParameters>();
            CreateMap<TheaterWorkerShortInformationDto, TheaterWorkerShortInformationModel>()
                .ForMember(destination => destination.PositionTypeName, options => options.MapFrom(c => c.PositionTypeName.GetEnumDisplayName()));

            CreateMap<PieceDto, PieceModel>();

            CreateMap<TheaterWorkerEntity, TheaterWorkerModel>()
                .ForMember(destination => destination.PositionTypeName, options => options.MapFrom(c => c.Position.PositionType.GetEnumDisplayName()))
                .ForMember(destination => destination.PositionName, options => options.MapFrom(c => c.Position.PositionName))
                .ForMember(destination => destination.PositionType, options => options.MapFrom(c => c.Position.PositionType));
            CreateMap<WriteResult<TheaterWorkerEntity>, WriteResult<TheaterWorkerModel>>();
        }
    }
}