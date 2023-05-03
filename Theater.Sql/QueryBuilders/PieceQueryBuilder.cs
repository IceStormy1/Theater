using LinqKit;
using System;
using System.Linq;
using System.Linq.Expressions;
using Theater.Abstractions.Filter;
using Theater.Entities.Theater;
using Theater.Sql.Extensions;

namespace Theater.Sql.QueryBuilders;

public sealed class PieceQueryBuilder : QueryBuilderBase<PieceEntity, PieceFilterSettings>
{
    public override Expression<Func<PieceEntity, bool>> BuildQueryFilter(PieceFilterSettings filter)
    {
        var pb = PredicateBuilder.New<PieceEntity>(x => true);

        pb.And(filter.GenreId, x => x.GenreId == filter.GenreId);
        pb.And(filter.Date, x => x.PieceDates.Any(x=>x.Date.Date == filter.Date.Value.Date));

        return pb;
    }

    protected override Expression<Func<PieceEntity, object>> ResolveSortingExpression(string sortColumn)
    {
        return sortColumn?.ToLowerInvariant() switch
        {
            "name" => x => x.PieceName,
            "genre" => x => x.Genre.GenreName,
            _ => x => x.PieceName
        };
    }
}