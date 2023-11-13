using AutoMapper;
using Microsoft.Extensions.Logging;
using Theater.Abstractions;
using Theater.Abstractions.UserReviews;
using Theater.Contracts.Theater.UserReview;
using Theater.Entities.Theater;

namespace Theater.Core.Theater.Services;

public sealed class UserReviewsService : BaseCrudService<UserReviewParameters, UserReviewEntity>, IUserReviewsService
{
    public UserReviewsService(
        IMapper mapper,
        ICrudRepository<UserReviewEntity> repository,
        IDocumentValidator<UserReviewParameters> documentValidator,
        ILogger<UserReviewsService> logger) : base(mapper, repository, documentValidator, logger)
    {
    }
}