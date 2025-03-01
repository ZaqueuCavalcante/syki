using Syki.Daemon.Commands;
using Syki.Daemon.Settings;

namespace Syki.Daemon.Configs;

public static class CommandsConfigs
{
    public static void AddCommandsConfigs(this IServiceCollection services)
    {
        services.AddSingleton<EmailSettings>();
        services.AddSingleton<DatabaseSettings>();
        services.AddSingleton<HangfireSettings>();

        services.AddTransient<CommandsProcessor>();
    }
}
