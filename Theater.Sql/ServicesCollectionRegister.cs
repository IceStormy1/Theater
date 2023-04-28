using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Theater.Sql;

public static class ServicesCollectionRegister
{
    public static void AddAllDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContextPool<TheaterDbContext>(x
            => x.UseNpgsql(configuration.GetConnectionString("Theater"), 
                options => options.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery)));
    }
}