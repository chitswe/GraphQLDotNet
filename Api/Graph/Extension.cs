
using Data;
using Data.Ef;
using GraphQL;
using GraphQL.DI;

namespace Api.Graph;
public static class Extension
{
    public static void AddApiGraphQL(this IServiceCollection services, Action<IGraphQLBuilder>? action = null)
    {
        services.AddScoped(typeof(EntityBatchDataLoader<>));
        services.AddSingleton<PageInfoGraphType>();
        services.AddSingleton<PaginationInputGraphType>();
        services.AddGraphQL(builder =>
        {
            builder
            .AddSystemTextJson()
            .AddSchema<RootSchema>()      
            .AddGraphTypes()      
            .ConfigureExecutionOptions(options =>
                {
                    var env = options.RequestServices?.GetRequiredService<IWebHostEnvironment>();
                    options.EnableMetrics = env?.IsDevelopment() ?? true;
                    var logger = options.RequestServices?.GetRequiredService<ILogger<WebApplication>>();
                    // options.UnhandledExceptionDelegate = ctx => { logger?.LogError("{Error} occurred", ctx.OriginalException.Message);return null; };
                })
            .AddErrorInfoProvider((opt, serviceProvider) => { opt.ExposeExceptionDetails = serviceProvider.GetRequiredService<IWebHostEnvironment>().IsDevelopment(); });
            if (action != null)
                action(builder);
        }
            );        
    }
}