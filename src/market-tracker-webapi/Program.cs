using market_tracker_webapi.Application.Http;
using market_tracker_webapi.Application.Http.Problem;
using market_tracker_webapi.Application.Pipeline;
using market_tracker_webapi.Application.Pipeline.authorization;
using market_tracker_webapi.Application.Service.DependencyResolver;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Batch;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace market_tracker_webapi;

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
        Console.WriteLine("Starting server...");
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

        app.UseODataBatching();

        app.UseExceptionHandler(_ => { });

        app.UseAuthorization();

        app.UseRouting();

        app.MapControllers();
    }

    private static void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.AddExceptionHandler<CustomExceptionHandler>();

        builder
            .Services.AddControllers(options =>
            {
                options.SuppressAsyncSuffixInActionNames = false;
                options.Filters.Add<AuthenticationFilter>();
                // options.ModelBinderProviders.Insert(0, new AuthUserBinderProvider());
            })
            .AddOData(options =>
            {
                var batchHandler = new DefaultODataBatchHandler
                {
                    MessageQuotas = { MaxPartsPerBatch = 20 } // TODO: find a way to change this value
                };
                options.AddRouteComponents(Uris.ApiBase, GetEdmModel(), batchHandler);
            });

        builder.Services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                return new BadRequestProblem.InvalidRequestContent(
                    context.ModelState.AsEnumerable().Last().Value?.Errors.First().ErrorMessage
                        ?? "Invalid request content."
                ).ToActionResult();
            };
        });

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddPgSqlServer(builder.Configuration);
        builder.Services.AddMarketTrackerDataServices();
    }

    private static IEdmModel GetEdmModel()
    {
        var builder = new ODataConventionModelBuilder();
        return builder.GetEdmModel();
    }
}
