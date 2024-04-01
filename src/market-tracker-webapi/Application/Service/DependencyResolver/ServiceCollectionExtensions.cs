using market_tracker_webapi.Application.Repository.Operations.Brand;
using market_tracker_webapi.Application.Repository.Operations.Category;
using market_tracker_webapi.Application.Repository.Operations.Product;
using market_tracker_webapi.Application.Repository.Operations.User;
using market_tracker_webapi.Application.Service.Operations.Category;
using market_tracker_webapi.Application.Service.Operations.Product;
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
            services.AddScoped<ITransactionManager, TransactionManager>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IBrandRepository, BrandRepository>();
            services.AddScoped<TransactionManager>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<ITokenRepository, TokenRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }
    }
}