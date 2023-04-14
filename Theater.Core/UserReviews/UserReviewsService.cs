using AutoMapper;
using Theater.Abstractions;
using Theater.Abstractions.UserReviews;
using Theater.Contracts.Theater;
using Theater.Entities.Theater;

namespace Theater.Core.UserReviews
{
    public class UserReviewsService : ServiceBase<UserReviewParameters, UserReviewEntity>, IUserReviewsService
    {
        public UserReviewsService(
            IMapper mapper,
            ICrudRepository<UserReviewEntity> repository,
            IDocumentValidator<UserReviewParameters> documentValidator) : base(mapper, repository, documentValidator)
        {
        }
    }
}
