using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using Theater.Abstractions;
using Theater.Entities;

namespace Theater.Sql.Repositories
{
    public class BaseCrudRepository<T, TDbContext> : ICrudRepository<T> where T : class, IEntity where TDbContext : DbContext
    {
        protected readonly TDbContext DbContext;
        protected readonly DbSet<T> DbSet;
        protected readonly ILogger<BaseCrudRepository<T, TDbContext>> Logger;

        public BaseCrudRepository(TDbContext dbContext,
            ILogger<BaseCrudRepository<T, TDbContext>> logger)
        {
            DbContext = dbContext;
            DbSet = dbContext.Set<T>();
            Logger = logger;
        }

        public Task<T> GetByEntityId(Guid id)
        {
            var query = DbSet.AsNoTracking().Where(x => x.Id == id);

            query = AddIncludes(query);

            return query.FirstOrDefaultAsync();
        }

        public async Task Add(T relation)
        {
            DbSet.Add(relation);
            await DbContext.SaveChangesAsync();
        }

        public virtual async Task Update(T relation)
        {
            var relationExists = await DbSet.AnyAsync(x => x.Id == relation.Id);

            if (relationExists == false)
            {
                Logger.LogWarning($"Entity '{relation.Id}' of type '{typeof(T)}' was not found on update attempt.");
                return;
            }

            DbSet.Update(relation);
            await DbContext.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            var relation = await DbSet.SingleOrDefaultAsync(x => x.Id == id);

            if (relation == null)
            {
                Logger.LogWarning($"Entity '{id}' of type '{typeof(T)}' was not found on delete attempt.");
                return;
            }

            DbSet.Remove(relation);
            await DbContext.SaveChangesAsync();
        }

        protected  virtual IQueryable<T> AddIncludes(IQueryable<T> query) => query;
    }
}
