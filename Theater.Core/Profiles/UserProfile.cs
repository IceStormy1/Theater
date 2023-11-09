using System;
using AutoMapper;
using Theater.Abstractions.Filters;
using Theater.Common.Enums;
using Theater.Contracts.Authorization;
using Theater.Contracts.FileStorage;
using Theater.Contracts.Filters;
using Theater.Contracts.UserAccount;
using Theater.Entities.Users;
using VkNet.Model;
using GenderType = Theater.Common.Enums.GenderType;

namespace Theater.Core.Profiles;

public sealed class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UserParameters, UserEntity>()
            .ForMember(destination => destination.CreatedAt, options => options.MapFrom(_ => DateTime.UtcNow))
            .ForMember(destination => destination.Money, options => options.MapFrom(_ => (decimal)default))
            .ForMember(destination => destination.RoleId, options => options.MapFrom(_ => (int)UserRole.User))
            .ForMember(destination => destination.PhotoId, options => options.MapFrom(x => x.Photo == null ? (Guid?)null : x.Photo.Id))
            .ForMember(destination => destination.Photo, options => options.Ignore())
            ;

        CreateMap<AccountSaveProfileInfoParams, UserEntity>()
            .ForMember(destination => destination.Id, options => options.Ignore())
            .ForMember(destination => destination.CreatedAt, options => options.MapFrom(_ => DateTime.UtcNow))
            .ForMember(destination => destination.RoleId, options => options.MapFrom(_ => (int)UserRole.User))
            .ForMember(destination => destination.Money, options => options.MapFrom(_ => (decimal)default))
            // .ForMember(destination => destination.VkId, options => options.MapFrom(exp => exp.Id))
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
            .ForMember(destination => destination.Photo, options => options.MapFrom(exp => exp.PhotoId.HasValue ? new StorageFileListItem { Id = exp.PhotoId.Value } : null))
            ;

        CreateMap<UserEntity, AuthenticateResponse>();
        CreateMap<UserAccountFilterParameters, UserAccountFilterSettings>();
        CreateMap<UserEntity, UserShortItem>();
        CreateMap<UserReviewFilterParameters, UserReviewFilterSettings>();
    }
}