using market_tracker_webapi.Application.Repository.Operations.User;
using market_tracker_webapi.Application.Service.Operations.User;
using market_tracker_webapi.Application.Service.Transaction;
using market_tracker_webapi.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace market_tracker_webapi.Application.Service.DependencyResolver
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPgSQLServer(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            services.AddDbContext<MarketTrackerDataContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("WebApiDatabase"))
            );

            return services;
        }

        public static IServiceCollection AddMarketTrackerDataServices(
            this IServiceCollection services
        )
        {
            services.AddScoped<TransactionManager>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRepository, UserRepository>();
            
            return services;
        }
    }
}