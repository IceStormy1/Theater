using AutoMapper;
using System;
using Theater.Abstractions.Authorization.Models;
using Theater.Contracts.Authorization;
using Theater.Entities.Authorization;

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
        }
    }
}