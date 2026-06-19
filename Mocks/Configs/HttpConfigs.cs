using System.Text.Json;
using Syki.Mocks.Providers;
using System.Text.Json.Serialization;

namespace Syki.Mocks.Configs;

public static class HttpConfigs
{
    public static void AddHttpConfigs(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers()
            .ConfigureApplicationPartManager(apm =>
                apm.FeatureProviders.Add(
                    new RegisterOnlyMocksControllersFeatureProvider()));

        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            });

        builder.Services.AddRouting(options => options.LowercaseUrls = true);
    }

    public static void UseControllers(this IApplicationBuilder app)
    {
        app.UseEndpoints(options =>
        {
            options.MapControllers();
        });
    }
}
