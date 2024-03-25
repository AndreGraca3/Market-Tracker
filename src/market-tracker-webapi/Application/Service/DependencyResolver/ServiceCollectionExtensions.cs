using market_tracker_webapi.Application.Repository;
using market_tracker_webapi.Application.Repository.EntityFramework;
using market_tracker_webapi.Application.Repository.Interfaces;
using market_tracker_webapi.Application.Service.Core;
using market_tracker_webapi.Application.Service.Transaction;
using market_tracker_webapi.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace market_tracker_webapi.Application.Service.DependencyResolver
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPgSqlServer(
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
            services.AddScoped<CategoryManager>(provider => new CategoryManager(3, 20));
            services.AddScoped<TransactionManager>();
            services.AddScoped<ProductService>();
            services.AddScoped<CategoryService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IBrandRepository, BrandRepository>();

            return services;
        }
    }
}
