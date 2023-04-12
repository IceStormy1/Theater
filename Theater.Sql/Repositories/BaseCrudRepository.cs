using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Theater.Abstractions;
using Theater.Entities;

namespace Theater.Sql.Repositories
{
    public class BaseCrudRepository<TEntity> : ICrudRepository<TEntity> 
        where TEntity : class, IEntity
    {
        protected readonly DbContext DbContext;
        protected readonly DbSet<TEntity> DbSet;
        protected readonly ILogger<BaseCrudRepository<TEntity>> Logger;

        public BaseCrudRepository(
            DbContext dbContext,
            ILogger<BaseCrudRepository<TEntity>> logger)
        {
            DbContext = dbContext;
            DbSet = dbContext.Set<TEntity>();
            Logger = logger;
        }

        public async Task<TEntity> GetByEntityId(Guid entityId)
        {
            var query = DbSet.AsNoTracking().Where(x => x.Id == entityId);

            query = AddIncludes(query);

            return await query.FirstOrDefaultAsync();
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
            DbSet.AddRange(entities);
            await DbContext.SaveChangesAsync();
        }

        public virtual async Task Update(TEntity entity)
        {
            var entityExists = await DbSet.AnyAsync(x => x.Id == entity.Id);

            if (entityExists == false)
            {
                Logger.LogWarning("Entity with entityIds {EntityId} of type '{EntityType}' was not found on update attempt.", 
                    entity.Id, typeof(TEntity));
                
                return;
            }

            DbSet.Update(entity);
            await DbContext.SaveChangesAsync();
        }

        public async Task UpdateRange(IReadOnlyCollection<TEntity> entities)
        {
            DbSet.UpdateRange(entities);
            await DbContext.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            var entity = await DbSet.SingleOrDefaultAsync(x => x.Id == id);

            if (entity == null)
            {
                Logger.LogWarning("Entity with {EntityId} of type '{EntityType}' was not found on delete attempt.",
                    id, typeof(TEntity));
               
                return;
            }

            DbSet.Remove(entity);
            await DbContext.SaveChangesAsync();
        }

        public async Task<bool> IsEntityExists(Guid id) => await DbSet.AnyAsync(x=>x.Id == id);

        public virtual IQueryable<TEntity> AddIncludes(IQueryable<TEntity> query) => query;
    }
}
