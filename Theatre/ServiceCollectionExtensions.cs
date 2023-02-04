using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Theater.Abstractions;
using Theater.Entities;
using Theater.Sql.Repositories;

namespace Theater
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRelationRepository<TEntity, TDbContext>(this IServiceCollection services)
            where TEntity : class, IEntity where TDbContext : DbContext
        {
            return services.AddScoped<ICrudRepository<TEntity>>(p =>
                new BaseCrudRepository<TEntity, TDbContext>(p.GetRequiredService<TDbContext>(), p.GetRequiredService<ILogger<BaseCrudRepository<TEntity, TDbContext>>>()));
        }
    }
}