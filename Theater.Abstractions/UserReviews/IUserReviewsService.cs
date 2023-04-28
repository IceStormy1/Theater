using Theater.Contracts.Theater;
using Theater.Entities.Theater;

namespace Theater.Abstractions.UserReviews;

public interface IUserReviewsService : ICrudService<UserReviewParameters, UserReviewEntity>
{
}