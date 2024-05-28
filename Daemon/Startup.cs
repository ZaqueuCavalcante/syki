using Syki.Daemon.Configs;

namespace Syki.Daemon;

public class Startup
{
    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddServicesConfigs();
    }

    public static void Configure(IApplicationBuilder app)
    {
    }
}
