using AutoMapper;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Theater.Abstractions;
using Theater.Abstractions.Filters;
using Theater.Common;
using Theater.Entities;

namespace Theater.Sql;

public class IndexReader<TModel, TEntity, TFilter> : IIndexReader<TModel, TEntity, TFilter>
    where TModel : class
    where TEntity : class, IEntity
    where TFilter : PagingSortSettings
{
    private readonly IQueryBuilder<TEntity, TFilter> _queryBuilder;
    private readonly DbSet<TEntity> _dbSet;
    private readonly bool _useAsExpandable;
    private readonly ICrudRepository<TEntity> _crudRepository;
    private readonly IMapper _mapper;

    /// <param name="dbContext">Контекст БД</param>
    /// <param name="queryBuilder">Реализация <see cref="IQueryBuilder{TEntity, TFilter}"/></param>
    /// <param name="crudRepository">Репозиторий сущности</param>
    /// <param name="mapper">Маппер</param>
    /// <param name="useAsExpandable">
    /// Использовать ли <see cref="Extensions.AsExpandable{T}(IQueryable{T})"/> вместо <see cref="Queryable.AsQueryable"/>.
    /// </param>
    /// <remarks>
    /// При использовании <paramref name="useAsExpandable"/> возможна проблема с Include
    /// </remarks>
    public IndexReader(DbContext dbContext,
        IQueryBuilder<TEntity, TFilter> queryBuilder, 
        ICrudRepository<TEntity> crudRepository,
        IMapper mapper,
        bool useAsExpandable = false)
    {
        _queryBuilder = queryBuilder;
        _crudRepository = crudRepository;
        _mapper = mapper;
        _dbSet = dbContext.Set<TEntity>();
        _useAsExpandable = useAsExpandable;
    }

    public async Task<PagingResult<TEntity>> QueryItems(TFilter filter)
    {
        if (filter == null)
            return await QueryItemsWithEmptyFilter();

        var table = _useAsExpandable ? _dbSet.AsExpandable() : _dbSet.AsQueryable();

        var predicate = _queryBuilder.BuildQueryFilter(filter);
        var query = table.Where(predicate);
        query = _crudRepository.AddIncludes(query);

        var total = await query.CountAsync();
        if (total == 0)
            return PagingResult<TEntity>.Empty;

        if (filter.Limit == 0 || filter.Offset >= total)
            return new PagingResult<TEntity>
            {
                Total = total,
                Items = Array.Empty<TEntity>()
            };

        var items = await _queryBuilder.ApplySortSettings(query, filter)
            .Skip(filter.Offset)
            .Take(filter.Limit)
            .AsNoTracking()
            .ToArrayAsync();

        return new PagingResult<TEntity>
        {
            Total = total,
            Items = items
        };
    }

    public async Task<WriteResult<TModel>> GetById(Guid id)
    {
        var entity = await _crudRepository.GetByEntityId(id);

        return entity is null 
            ? WriteResult<TModel>.FromError(ErrorModel.Default("delete-conflict", "Указанная запись не найдена")) 
            : WriteResult.FromValue(_mapper.Map<TModel>(entity));
    }

    private async Task<PagingResult<TEntity>> QueryItemsWithEmptyFilter()
    {
        var total = await _dbSet.CountAsync();
        if (total == 0)
            return PagingResult<TEntity>.Empty;

        var items = await _dbSet
            .Take(PagingSettings.DefaultLimit)
            .AsNoTracking()
            .ToArrayAsync();

        return new PagingResult<TEntity>
        {
            Total = total,
            Items = items
        };
    }
}