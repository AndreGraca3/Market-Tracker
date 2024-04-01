using market_tracker_webapi.Application.Repository.Operations.City;
using market_tracker_webapi.Application.Repository.Operations.Company;
using market_tracker_webapi.Application.Repository.Operations.Store;
using market_tracker_webapi.Application.Repository.Operations.User;
using market_tracker_webapi.Application.Service.Operations.City;
using market_tracker_webapi.Application.Service.Operations.Company;
using market_tracker_webapi.Application.Service.Operations.Store;
using market_tracker_webapi.Application.Services.Transaction;
using market_tracker_webapi.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace market_tracker_webapi.Application.Service.DependencyResolver
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
            services.AddScoped<ITransactionManager, TransactionManager>();
            
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IStoreRepository, StoreRepository>();
            services.AddScoped<ICityRepository, CityRepository>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            
            //services.AddScoped<IUserService, UserService>();
            services.AddScoped<IStoreService, StoreService>();
            services.AddScoped<ICityService, CityService>();
            services.AddScoped<ICompanyService, CompanyService>();

            return services;
        }
    }
}
