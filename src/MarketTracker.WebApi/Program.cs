using MarketTracker.WebApi.Application.Services.DependencyResolver;

namespace MarketTracker
{
    static class Program
    {
        public static void Main(string[] args)
        {
            Run(args);
        }

        private static void Run(string[] args)
        {
            WebApplication app = CreateWebHostBuilder(args).Build();
            Configure(app);
            app.Run();
        }

        private static WebApplicationBuilder CreateWebHostBuilder(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
            ConfigureServices(builder);
            return builder;
        }

        private static void Configure(WebApplication app)
        {
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "swagger/{documentName}/swagger.json";
            });


            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint($"/swagger/v1/swagger.json", "v1");
                options.RoutePrefix = "swagger";
            });

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();

        }

        private static void ConfigureServices(WebApplicationBuilder builder)
        {
            builder.Services.AddControllers(options => options.SuppressAsyncSuffixInActionNames = false);
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // builder.Services.AddAzureSqlServer(builder.Configuration);
            builder.Services.AddMarketTrackerDataServices();
        }
    }

}