using market_tracker_webapi.Application.Repository;
using market_tracker_webapi.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace market_tracker_webapi.Application.Services.DependencyResolver
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPgSQLServer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<MarketTrackerDataContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("WebApiDatabase")));

            return services;
        }

        public static IServiceCollection AddMarketTrackerDataServices(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }
    }
}
