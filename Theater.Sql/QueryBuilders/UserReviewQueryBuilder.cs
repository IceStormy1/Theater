using LinqKit;
using System;
using System.Linq.Expressions;
using Theater.Abstractions.Filters;
using Theater.Entities.Theater;
using Theater.Sql.Extensions;

namespace Theater.Sql.QueryBuilders
{
    public sealed class UserReviewQueryBuilder : QueryBuilderBase<UserReviewEntity, UserReviewFilterSettings>
    {
        public override Expression<Func<UserReviewEntity, bool>> BuildQueryFilter(UserReviewFilterSettings filter)
        {
            var pb = PredicateBuilder.New<UserReviewEntity>(x => true);

            pb.And(filter.PieceId, x => x.PieceId == filter.PieceId);
            pb.And(filter.UserId, x => x.UserId == filter.UserId);

            return pb;
        }

        protected override Expression<Func<UserReviewEntity, object>> ResolveSortingExpression(string sortColumn)
        {
            return sortColumn?.ToLowerInvariant() switch
            {
                "title" => x => x.Title,
                "piecename" => x => x.Piece.PieceName,
                "username" => x => x.User.UserName,
                _ => x => x.Title
            };
        }
    }
}
