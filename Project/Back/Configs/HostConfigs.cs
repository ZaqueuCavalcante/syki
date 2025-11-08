namespace Exato.Back.Configs;

public static class HostConfigs
{
    public static void AddHostConfigs(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((context, config) => 
            config.ReadFrom.Configuration(context.Configuration)
        );
    }
}
