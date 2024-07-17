using System.Diagnostics.CodeAnalysis;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using market_tracker_webapi.Application.Http.Pipeline.Authorization;
using market_tracker_webapi.Application.Repository.Account.Credential;
using market_tracker_webapi.Application.Repository.Account.Token;
using market_tracker_webapi.Application.Repository.Account.Users.Client;
using market_tracker_webapi.Application.Repository.Account.Users.Operator;
using market_tracker_webapi.Application.Repository.Account.Users.User;
using market_tracker_webapi.Application.Repository.List;
using market_tracker_webapi.Application.Repository.List.ListEntry;
using market_tracker_webapi.Application.Repository.Market.Alert;
using market_tracker_webapi.Application.Repository.Market.City;
using market_tracker_webapi.Application.Repository.Market.Company;
using market_tracker_webapi.Application.Repository.Market.Inventory.Brand;
using market_tracker_webapi.Application.Repository.Market.Inventory.Category;
using market_tracker_webapi.Application.Repository.Market.Inventory.Product;
using market_tracker_webapi.Application.Repository.Market.Price;
using market_tracker_webapi.Application.Repository.Market.Store;
using market_tracker_webapi.Application.Repository.Market.Store.PreRegister;
using market_tracker_webapi.Application.Service.Errors.PreRegister;
using market_tracker_webapi.Application.Service.External;
using market_tracker_webapi.Application.Service.Operations.Account.Auth.GoogleAuth;
using market_tracker_webapi.Application.Service.Operations.Account.Auth.PreRegister;
using market_tracker_webapi.Application.Service.Operations.Account.Auth.Token;
using market_tracker_webapi.Application.Service.Operations.Account.Users.Client;
using market_tracker_webapi.Application.Service.Operations.Account.Users.Operator;
using market_tracker_webapi.Application.Service.Operations.Account.Users.User;
using market_tracker_webapi.Application.Service.Operations.List;
using market_tracker_webapi.Application.Service.Operations.Market.Alert;
using market_tracker_webapi.Application.Service.Operations.Market.City;
using market_tracker_webapi.Application.Service.Operations.Market.Company;
using market_tracker_webapi.Application.Service.Operations.Market.Inventory.Category;
using market_tracker_webapi.Application.Service.Operations.Market.Inventory.Product;
using market_tracker_webapi.Application.Service.Operations.Market.Store;
using market_tracker_webapi.Application.Service.Transaction;
using market_tracker_webapi.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace market_tracker_webapi.Application.Service.DependencyResolver
{
    [ExcludeFromCodeCoverage]
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

        public static IServiceCollection AddFirebaseServices(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            FirebaseApp.Create(
                new AppOptions
                {
                    Credential = GoogleCredential.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                        configuration["FirebaseServiceAccountJsonFile"] ?? throw new InvalidOperationException(
                            "FirebaseServiceAccountJsonFile is not set in appsettings.json"))
                    )
                }
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

            services.AddScoped<IAlertService, AlertService>();
            services.AddScoped<IPriceAlertRepository, PriceAlertRepository>();

            services.AddScoped<RequestTokenProcessor>();
            services.AddScoped<IGoogleAuthService, GoogleAuthService>();
            services.AddScoped<INotificationService, NotificationService>();

            services.AddScoped<IAccountRepository, AccountRepository>();

            services.AddScoped<IClientService, ClientService>();
            services.AddScoped<IClientRepository, ClientRepository>();

            services.AddScoped<IClientDeviceService, ClientDeviceService>();
            services.AddScoped<IClientDeviceRepository, ClientDeviceRepository>();

            services.AddScoped<IOperatorService, OperatorService>();
            services.AddScoped<IOperatorRepository, OperatorRepository>();

            services.AddScoped<IPreRegistrationRepository, PreRegistrationRepository>();
            services.AddScoped<IPreRegistrationService, PreRegistrationService>();

            return services;
        }

        public static IServiceCollection AddGoogleAuthAuthentication(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            return services;
        }
    }
}