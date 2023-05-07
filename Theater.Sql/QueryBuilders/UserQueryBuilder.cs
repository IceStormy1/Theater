using LinqKit;
using System;
using System.Linq.Expressions;
using Theater.Abstractions.Filters;
using Theater.Entities.Authorization;
using Theater.Sql.Extensions;

namespace Theater.Sql.QueryBuilders;

public sealed class UserQueryBuilder : QueryBuilderBase<UserEntity, UserAccountFilterSettings>
{
    public override Expression<Func<UserEntity, bool>> BuildQueryFilter(UserAccountFilterSettings filter)
    {
        var pb = PredicateBuilder.New<UserEntity>(x => true);

        pb.And(filter.FirstName, x => x.FirstName.Contains(filter.FirstName));

        return pb;
    }

    protected override Expression<Func<UserEntity, object>> ResolveSortingExpression(string sortColumn)
    {
        return sortColumn?.ToLowerInvariant() switch
        {
            "firstname" => x => x.FirstName,
            "middlename" => x => x.MiddleName,
            "lastname" => x => x.LastName,
            _ => x => x.FirstName
        };
    }
}