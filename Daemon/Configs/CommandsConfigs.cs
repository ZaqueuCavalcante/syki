using Syki.Daemon.Commands;
using Syki.Daemon.Settings;

namespace Syki.Daemon.Configs;

public static class CommandsConfigs
{
    public static void AddCommandsConfigs(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<EmailSettings>();
        builder.Services.AddSingleton<DatabaseSettings>();
        builder.Services.AddSingleton<HangfireSettings>();

        builder.Services.AddTransient<CommandsProcessor>();
    }
}
