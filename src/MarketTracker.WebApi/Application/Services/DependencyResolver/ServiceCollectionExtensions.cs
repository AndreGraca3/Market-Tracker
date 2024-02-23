using MarketTracker.WebApi.Application.Queries;

namespace MarketTracker.WebApi.Application.Services.DependencyResolver
{
    public static class ServiceCollectionExtensions
    {
        /*public static IServiceCollection AddAzureSqlServer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<MarketTrackerDataContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("SqlServerConnectionString")));

            return services;
        }*/

        public static IServiceCollection AddMarketTrackerDataServices(this IServiceCollection services)
        {
            services.AddScoped<IHomeQuery, HomeQuery>();

            return services;
        }
    }
}
