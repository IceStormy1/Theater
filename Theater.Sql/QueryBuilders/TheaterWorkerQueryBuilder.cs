using LinqKit;
using System;
using System.Linq.Expressions;
using Theater.Abstractions.Filters;
using Theater.Entities.Theater;
using Theater.Sql.Extensions;

namespace Theater.Sql.QueryBuilders;

public sealed class TheaterWorkerQueryBuilder : QueryBuilderBase<TheaterWorkerEntity, TheaterWorkerFilterSettings>
{
    public override Expression<Func<TheaterWorkerEntity, bool>> BuildQueryFilter(TheaterWorkerFilterSettings filter)
    {
        var pb = PredicateBuilder.New<TheaterWorkerEntity>(x => true);

        pb.And(filter.PositionTypeId, x => x.Position.PositionType == filter.PositionTypeId);

        return pb;
    }

    protected override Expression<Func<TheaterWorkerEntity, object>> ResolveSortingExpression(string sortColumn)
    {
        return sortColumn?.ToLowerInvariant() switch
        {
            "name" => x => x.FirstName,
            "position" => x => x.Position.PositionName,
            _ => x => x.FirstName
        };
    }
}