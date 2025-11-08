using Exato.Mocks.Providers;

namespace Exato.Mocks.Configs;

public static class HttpConfigs
{
    public static void AddHttpConfigs(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers()
            .ConfigureApplicationPartManager(apm =>
                apm.FeatureProviders.Add(
                    new RegisterOnlyMocksControllersFeatureProvider()));

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
