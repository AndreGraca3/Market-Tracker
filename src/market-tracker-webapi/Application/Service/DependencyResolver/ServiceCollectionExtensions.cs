using market_tracker_webapi.Application.Repository.Operations.Brand;
using market_tracker_webapi.Application.Repository.Operations.Category;
using market_tracker_webapi.Application.Repository.Operations.City;
using market_tracker_webapi.Application.Repository.Operations.Company;
using market_tracker_webapi.Application.Repository.Operations.Prices;
using market_tracker_webapi.Application.Repository.Operations.List;
using market_tracker_webapi.Application.Repository.Operations.Product;
using market_tracker_webapi.Application.Repository.Operations.Store;
using market_tracker_webapi.Application.Repository.Operations.Token;
using market_tracker_webapi.Application.Repository.Operations.User;
using market_tracker_webapi.Application.Service.Operations.Category;
using market_tracker_webapi.Application.Service.Operations.City;
using market_tracker_webapi.Application.Service.Operations.Company;
using market_tracker_webapi.Application.Service.Operations.List;
using market_tracker_webapi.Application.Service.Operations.Product;
using market_tracker_webapi.Application.Service.Operations.Store;
using market_tracker_webapi.Application.Service.Operations.Token;
using market_tracker_webapi.Application.Service.Operations.User;
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
                options.UseNpgsql(configuration.GetConnectionString("WebApiDatabase")
                )
            );

            return services;
        }

        public static IServiceCollection AddMarketTrackerDataServices(
            this IServiceCollection services
        )
        {
            services.AddScoped<ITransactionManager, TransactionManager>();
            
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProductRepository, ProductRepository>();
            
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            
            services.AddScoped<IBrandRepository, BrandRepository>();
            
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<ITokenRepository, TokenRepository>();
            
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRepository, UserRepository>();
            
            services.AddScoped<IStoreService, StoreService>();
            services.AddScoped<IStoreRepository, StoreRepository>();
            
            services.AddScoped<ICityService, CityService>();
            services.AddScoped<ICityRepository, CityRepository>();
            
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            
            services.AddScoped<IListService, ListService>();
            services.AddScoped<IListRepository, ListRepository>();

            services.AddScoped<IListEntryService, ListEntryService>();
            services.AddScoped<IListEntryRepository, ListEntryRepository>();
            
            services.AddScoped<IProductFeedbackService, ProductFeedbackService>();
            services.AddScoped<IProductFeedbackRepository, ProductFeedbackRepository>();
            
            services.AddScoped<IProductPriceService, ProductPriceService>();
            services.AddScoped<IPriceRepository, PriceRepository>();
            
            return services;
        }

        public static IServiceCollection AddGoogleAuthAuthentication(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            services.AddAuthentication().AddGoogle(googleOptions =>
            {
                googleOptions.ClientId = configuration["Authentication:Google:ClientId"];
                googleOptions.ClientSecret = configuration["Authentication:Google:ClientSecret"];
            });

            return services;
        }
    }
}