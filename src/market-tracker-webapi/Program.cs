using System.Text.Json.Serialization;
using market_tracker_webapi.Application.Http;
using market_tracker_webapi.Application.Http.Problem;
using market_tracker_webapi.Application.Pipeline;
using market_tracker_webapi.Application.Pipeline.authorization;
using market_tracker_webapi.Application.Service.DependencyResolver;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Batch;
using Microsoft.AspNetCore.OData.Routing.Conventions;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;

namespace market_tracker_webapi;

static class Program
{
    public static void Main(string[] args)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        Run(args);
    }

    private static void Run(string[] args)
    {
        WebApplication app = CreateWebHostBuilder(args).Build();
        Configure(app);
        Console.WriteLine("Started server...");
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
        app.UseSwagger(c => { c.RouteTemplate = "swagger/{documentName}/swagger.json"; });

        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint($"/swagger/v1/swagger.json", "v1");
            options.RoutePrefix = "swagger";
        });

        app.UseODataBatching();

        app.Use(
            async (context, next) =>
            {
                var startTime = DateTime.Now;
                Console.WriteLine($"Request: {context.Request.Method} {context.Request.Path}");
                await next();
                Console.WriteLine(
                    $"Response: {context.Response.StatusCode} in {(DateTime.Now - startTime).TotalMilliseconds}ms"
                );
            }
        );

        app.UseExceptionHandler(_ => { });

        app.UseAuthorization();

        app.UseRouting();

        app.MapControllers();
    }

    private static void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.AddExceptionHandler<CustomExceptionHandler>();

        var batchHandler = new DefaultODataBatchHandler();
        batchHandler.MessageQuotas.MaxOperationsPerChangeset = 10;
        batchHandler.MessageQuotas.MaxPartsPerBatch = 1000;
        builder
            .Services.AddControllers(options =>
            {
                options.SuppressAsyncSuffixInActionNames = false;
                options.Filters.Add<AuthorizationFilter>();
            })
            .AddJsonOptions(o =>
            {
                o.JsonSerializerOptions.Converters.Add(new OptionalConverter());
                o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            })
            .AddOData(options =>
            {
                options.Conventions.Remove(
                    options.Conventions.OfType<MetadataRoutingConvention>().First()
                );
                options.AddRouteComponents(Uris.ApiBase, GetEdmModel(), batchHandler);
            });

        builder.Services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
                new BadRequestProblem.InvalidRequestContent(
                    context
                        .ModelState.Values.SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .FirstOrDefault() ?? "Invalid request content"
                ).ToActionResult();
        });

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Token", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Name = AuthenticationDetails.NameAuthorizationHeader,
                Description = "API token",
                Type = SecuritySchemeType.ApiKey,
                BearerFormat = "UUID",
                Scheme = "bearer"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Name = AuthenticationDetails.NameAuthorizationHeader,
                        In = ParameterLocation.Header,
                        Reference = new OpenApiReference
                        {
                            Id = "Token",
                            Type = ReferenceType.SecurityScheme
                        }
                    },
                    new List<string>()
                }
            });
        });

        builder.Services.AddFirebaseServices(builder.Configuration);
        builder.Services.AddPgSqlServer(builder.Configuration);
        builder.Services.AddMarketTrackerDataServices();
        builder.Services.AddGoogleAuthAuthentication(builder.Configuration);
    }

    private static IEdmModel GetEdmModel()
    {
        var builder = new ODataConventionModelBuilder();
        return builder.GetEdmModel();
    }
}