using Syki.Configs;
using Syki.Settings;

namespace Syki;

public class Startup
{
    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<DatabaseSettings>();

        services.AddControllers();

        services.AddRouting(options => options.LowercaseUrls = true);

        services.AddEfCoreConfigs();

        services.AddSwaggerConfigs();
    }

    public static void Configure(IApplicationBuilder app)
    {
        app.UseRouting();

        app.UseSwaggerThings();

        app.UseEndpoints(options => options.MapControllers());
    }
}
