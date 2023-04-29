using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Theater.Abstractions;
using Theater.Core;
using Theater.Entities;
using Theater.Sql;
using Theater.Sql.Repositories;

namespace Theater;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRelationRepository<TEntity, TDbContext>(this IServiceCollection services)
        where TEntity : class, IEntity 
        where TDbContext : TheaterDbContext
    {
        return services.AddScoped<ICrudRepository<TEntity>>(p =>
            new BaseCrudRepository<TEntity>(p.GetRequiredService<TDbContext>(), p.GetRequiredService<ILogger<BaseCrudRepository<TEntity>>>()));
    }

    public static IServiceCollection AddCrudService<TDocumentModel, TEntity>(this IServiceCollection services)
        where TDocumentModel : class
        where TEntity : class, IEntity, new()
    {
        return services.AddScoped<ICrudService<TDocumentModel, TEntity>, ServiceBase<TDocumentModel, TEntity>>();
    }
        
    public static IServiceCollection AddStubValidator<T>(this IServiceCollection services) where T : class
        => services.AddSingleton<IDocumentValidator<T>, DocumentValidatorStub<T>>();
}