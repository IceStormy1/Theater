using AutoMapper;
using Microsoft.IdentityModel.JsonWebTokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Theater.Abstractions.Filters;
using Theater.Common.Enums;
using Theater.Common.Extensions;
using Theater.Contracts.Authorization;
using Theater.Contracts.FileStorage;
using Theater.Contracts.Filters;
using Theater.Contracts.Messages;
using Theater.Contracts.UserAccount;
using Theater.Entities.Users;
using GenderType = Theater.Common.Enums.GenderType;

namespace Theater.Core.Profiles;

public sealed class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UserParameters, UserEntity>()
            .ForMember(destination => destination.CreatedAt, options => options.MapFrom(_ => DateTime.UtcNow))
            .ForMember(destination => destination.Money, options => options.MapFrom(_ => (decimal)default))
            .ForMember(destination => destination.Role, options => options.MapFrom(_ => (int)UserRole.User))
            .ForMember(destination => destination.PhotoId, options => options.MapFrom(x => x.Photo == null ? (Guid?)null : x.Photo.Id))
            .ForMember(destination => destination.Photo, options => options.Ignore())
            ;

        CreateMap<UserModel, UserEntity>()
            .IncludeBase<UserParameters, UserEntity>()
            .ForMember(destination => destination.Id, options => options.Ignore())
            ;

        CreateMap<UserEntity, UserModel>()
            .ForMember(destination => destination.Photo, options => options.MapFrom(exp => exp.PhotoId.HasValue ? new StorageFileListItem { Id = exp.PhotoId.Value } : null))
            ;

        CreateMap<UserEntity, AuthenticateResponse>();
        CreateMap<UserAccountFilterParameters, UserAccountFilterSettings>();
        CreateMap<UserEntity, UserShortItem>();
        CreateMap<UserReviewFilterParameters, UserReviewFilterSettings>();

        CreateMap<UserEntity, AuthorDto>()
            .ForMember(destination => destination.Photo, options => options.MapFrom(exp => exp.PhotoId.HasValue ? new StorageFileListItem { Id = exp.PhotoId.Value } : null))
            ;

        CreateMap<IEnumerable<Claim>, UserModel>()
            .ForMember(dest => dest.Email, act => act.MapFrom(src => src.FirstOrDefault(s => s.Type == JwtRegisteredClaimNames.Email).Value))
            .ForMember(dest => dest.Phone, act => act.MapFrom(src => src.FirstOrDefault(s => s.Type == JwtRegisteredClaimNames.PhoneNumber).Value))
            .ForMember(dest => dest.LastName, act => act.MapFrom(src => src.FirstOrDefault(s => s.Type == JwtRegisteredClaimNames.FamilyName).Value))
            .ForMember(dest => dest.FirstName, act => act.MapFrom(src => src.FirstOrDefault(s => s.Type == JwtRegisteredClaimNames.GivenName).Value))
            .ForMember(dest => dest.UserName, act => act.MapFrom(src => src.FirstOrDefault(s => s.Type == JwtRegisteredClaimNames.Name).Value))
            .ForMember(dest => dest.MiddleName, act => act.MapFrom(src => src.FirstOrDefault(s => s.Type == "middle_name").Value))
            .ForMember(dest => dest.BirthDate, act => act.MapFrom(src => src.FirstOrDefault(s => s.Type == JwtRegisteredClaimNames.Birthdate).Value))
            .ForMember(dest => dest.Gender, act => act.MapFrom(src => GetGenderFromClaims(src)))
            .ForMember(dest => dest.Snils, act => act.MapFrom(src => src.FirstOrDefault(s => s.Type == "snils").Value))
            .ForMember(dest => dest.Role, act => act.MapFrom(src => src.FirstOrDefault(s => s.Type == "role").Value.ConvertStringToEnum<UserRole>()))
            .ForMember(dest => dest.ExternalUserId, act => act.MapFrom(src => src.FirstOrDefault(s => s.Type == JwtRegisteredClaimNames.Sub).Value))
            ;

    }

    private static GenderType GetGenderFromClaims(IEnumerable<Claim> claims)
        => Enum.TryParse<GenderType>(claims.FirstOrDefault(s => s.Type == JwtRegisteredClaimNames.Gender)?.Value, out var gender)
            ? gender
            : GenderType.Male;
}