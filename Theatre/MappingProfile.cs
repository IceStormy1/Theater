using System;
using AutoMapper;
using Theater.Contracts.Authorization;
using Theater.Entities.Authorization;

namespace Theater
{
    internal class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserParameters, UserEntity>()
                .ForMember(destination => destination.DateOfCreate, options => options.MapFrom(_ => DateTime.UtcNow));

            CreateMap<UserEntity, UserModel>()
                .ForMember(destination => destination.Password, options => options.MapFrom(c => c.Password));

            CreateMap<UserEntity, AuthenticateResponse>();
        }
    }
}
