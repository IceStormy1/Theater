using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Theater.Abstractions.UserReviews;
using Theater.Entities.Theater;

namespace Theater.Sql.Repositories
{
    public sealed class UserReviewsRepository : BaseCrudRepository<UserReviewEntity>, IUserReviewsRepository
    {
        public UserReviewsRepository(
            TheaterDbContext dbContext,
            ILogger<BaseCrudRepository<UserReviewEntity>> logger) : base(dbContext, logger)
        {
        }

        public override IQueryable<UserReviewEntity> AddIncludes(IQueryable<UserReviewEntity> query)
        {
            return query
                .Include(x => x.Piece)
                .Include(x => x.User);
        }
    }
}
