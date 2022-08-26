namespace RecipeManagement.Extensions.Services;

using RecipeManagement.Middleware;
using RecipeManagement.Services;
using System.Text.Json.Serialization;
using Serilog;
using FluentValidation.AspNetCore;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Resources;
using Sieve.Services;
using System.Reflection;

public static class WebAppServiceConfiguration
{
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton(Log.Logger);
        // TODO update CORS for your env
        builder.Services.AddCorsService("RecipeManagementCorsPolicy", builder.Environment);
        builder.Services.OpenTelemetryRegistration("RecipeManagement");
        builder.Services.AddInfrastructure(builder.Environment);
        builder.Services.AddMassTransitServices(builder.Environment);

        // using Newtonsoft.Json to support PATCH docs since System.Text.Json does not support them https://github.com/dotnet/aspnetcore/issues/24333
        // if you are not using PatchDocs and would prefer to use System.Text.Json, you can remove The `AddNewtonSoftJson()` line
        builder.Services.AddControllers(options => options.UseDateOnlyTimeOnlyStringConverters())
            .AddJsonOptions(options => options.UseDateOnlyTimeOnlyStringConverters())
            .AddJsonOptions(o => o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles)
            .AddNewtonsoftJson();
        builder.Services.AddApiVersioningExtension();

        builder.Services.AddHttpContextAccessor();
        builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
        builder.Services.AddScoped<SieveProcessor>();

        // registers all services that inherit from your base service interface - IRecipeManagementService
        builder.Services.AddBoundaryServices(Assembly.GetExecutingAssembly());

        builder.Services.AddMvc(options => options.Filters.Add<ErrorHandlerFilterAttribute>());

        if(builder.Environment.EnvironmentName != Consts.Testing.FunctionalTestingEnvName)
        {
            var typeAdapterConfig = TypeAdapterConfig.GlobalSettings;
            typeAdapterConfig.Scan(Assembly.GetExecutingAssembly());
            var mapperConfig = new Mapper(typeAdapterConfig);
            builder.Services.AddSingleton<IMapper>(mapperConfig);
        }

        builder.Services.AddHealthChecks();
        builder.Services.AddSwaggerExtension();
    }

    /// <summary>
    /// Registers all services in the assembly of the given interface.
    /// </summary>
    private static void AddBoundaryServices(this IServiceCollection services, params Assembly[] assemblies)
    {
        if (!assemblies.Any())
            throw new ArgumentException("No assemblies found to scan. Supply at least one assembly to scan for handlers.");

        foreach (var assembly in assemblies)
        {
            var rules = assembly.GetTypes()
                .Where(x => !x.IsAbstract && x.IsClass && x.GetInterface(nameof(IRecipeManagementService)) == typeof(IRecipeManagementService));

            foreach (var rule in rules)
            {
                foreach (var @interface in rule.GetInterfaces())
                {
                    services.Add(new ServiceDescriptor(@interface, rule, ServiceLifetime.Scoped));
                }
            }
        }
    }
}
