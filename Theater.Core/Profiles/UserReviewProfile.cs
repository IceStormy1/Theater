using AutoMapper;
using Theater.Abstractions.Filters;
using Theater.Contracts.Filters;
using Theater.Contracts.Theater.UserReview;
using Theater.Entities.Theater;

namespace Theater.Core.Profiles;

public sealed class UserReviewProfile : Profile
{
    public UserReviewProfile()
    {
        CreateMap<UserReviewFilterParameters, UserReviewFilterSettings>();
        CreateMap<UserReviewParameters, UserReviewEntity>();
        CreateMap<UserReviewEntity, UserReviewModel>()
            .ForMember(destination => destination.UserName, options => options.MapFrom(exp => exp.User == null ? null : exp.User.UserName))
            .ForMember(destination => destination.PieceName, options => options.MapFrom(exp => exp.User == null ? null : exp.Piece.PieceName))
            ;
    }
}