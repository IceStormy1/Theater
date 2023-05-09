using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Theater.Abstractions;
using Theater.Entities;

namespace Theater.Sql.Repositories;

public class BaseCrudRepository<TEntity> : ICrudRepository<TEntity> 
    where TEntity : class, IEntity
{
    protected readonly TheaterDbContext DbContext;
    protected readonly DbSet<TEntity> DbSet;
    protected readonly ILogger<BaseCrudRepository<TEntity>> Logger;

    public BaseCrudRepository(
        TheaterDbContext dbContext,
        ILogger<BaseCrudRepository<TEntity>> logger)
    {
        DbContext = dbContext;
        DbSet = dbContext.Set<TEntity>();
        Logger = logger;
    }

    public async Task<TEntity> GetByEntityId(Guid entityId, bool useAsNoTracking = false)
    {
        var query = DbSet.AsQueryable();

        query = AddIncludes(query);

        if (useAsNoTracking)
            query = query.AsNoTracking();

        return await query.FirstOrDefaultAsync(x => x.Id == entityId);
    }

    public async Task<IReadOnlyCollection<TEntity>> GetByEntityIds(IReadOnlyCollection<Guid> entityIds)
    {
        var query = DbSet.AsNoTracking().Where(x => entityIds.Contains(x.Id));

        query = AddIncludes(query);

        return await query.ToListAsync();
    }

    public async Task Add(TEntity entity)
    {
        DbSet.Add(entity);
        await DbContext.SaveChangesAsync();
    }

    public async Task AddRange(IReadOnlyCollection<TEntity> entities)
    {
        var transaction = await DbContext.Database.BeginTransactionAsync();

        try
        {
            DbSet.AddRange(entities);
            await DbContext.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Произошла ошибка при добавлении записей {Type}", typeof(TEntity).Name);
           
            throw;
        }
    }

    public virtual async Task Update(TEntity entity)
    {
        var entityExists = await DbSet.AnyAsync(x => x.Id == entity.Id);

        if (entityExists == false)
        {
            Logger.LogWarning("Сущность с идентификатором {Id} с типом '{EntityType}' не найдена при обновлении.", 
                entity.Id, typeof(TEntity));
                
            return;
        }

        DbSet.Update(entity);
        await DbContext.SaveChangesAsync();
    }

    public async Task UpdateRange(IReadOnlyCollection<TEntity> entities)
    {
        var transaction = await DbContext.Database.BeginTransactionAsync();

        try
        {
            DbSet.UpdateRange(entities);
            await DbContext.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Произошла ошибка при обновлении записей {Type}", typeof(TEntity).Name);
           
            throw;
        }
    }

    public async Task Delete(Guid id)
    {
        var entity = await DbSet.SingleOrDefaultAsync(x => x.Id == id);

        if (entity == null)
        {
            Logger.LogWarning("Сущность с идентификатором {Id} типа '{EntityType}' не найдена при удалении.",
                id, typeof(TEntity));
               
            return;
        }

        DbSet.Remove(entity);
        await DbContext.SaveChangesAsync();
    }

    public async Task<bool> IsEntityExists(Guid id) => await DbSet.AnyAsync(x=>x.Id == id);

    public virtual IQueryable<TEntity> AddIncludes(IQueryable<TEntity> query) => query;
}