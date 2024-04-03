using Audit.Core;
using Syki.Back.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;

namespace Syki.Daemon.Configs;

public static class AppConfigs
{
    public static void AddAppConfigs(this IHostBuilder builder)
    {
        Configuration.AuditDisabled = true;

        builder.ConfigureAppConfiguration(config =>
        {
            var configPath = Path.Combine(Directory.GetCurrentDirectory(), $"appsettings.{Env.Get()}.json");

            var configuration = new ConfigurationBuilder()
                .AddJsonFile(configPath)
                .Build();

            config.AddConfiguration(configuration);
        });
    }
}
