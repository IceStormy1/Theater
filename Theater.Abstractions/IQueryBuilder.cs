using System;
using System.Linq;
using System.Linq.Expressions;
using Theater.Abstractions.Filter;

namespace Theater.Abstractions
{
    public interface IQueryBuilder<TEntity, in TFilter>
        where TEntity : class
        where TFilter : PagingSortSettings
    {
        Expression<Func<TEntity, bool>> BuildQueryFilter(TFilter filter);

        IQueryable<TEntity> ApplySortSettings(IQueryable<TEntity> query, PagingSortSettings pagingSettings);
    }
}
