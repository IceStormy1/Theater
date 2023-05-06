using System;
using System.Linq;
using System.Linq.Expressions;
using Theater.Abstractions;
using Theater.Abstractions.Filters;
using Theater.Entities;

namespace Theater.Sql;

public abstract class QueryBuilderBase<TEntity, TFilter> : IQueryBuilder<TEntity, TFilter>
    where TEntity : class, IEntity
    where TFilter : PagingSortSettings
{
    public abstract Expression<Func<TEntity, bool>> BuildQueryFilter(TFilter filter);

    public IQueryable<TEntity> ApplySortSettings(IQueryable<TEntity> query, PagingSortSettings pagingSettings)
    {
        if (string.IsNullOrEmpty(pagingSettings.SortColumn))
            return query;

        var expression = ResolveSortingExpression(pagingSettings.SortColumn);
        if (expression == null)
            return query;

        return pagingSettings.SortOrder == SortOrder.Desc
            ? query.OrderByDescending(expression)
            : query.OrderBy(expression);
    }

    protected abstract Expression<Func<TEntity, object>> ResolveSortingExpression(string sortColumn);
}