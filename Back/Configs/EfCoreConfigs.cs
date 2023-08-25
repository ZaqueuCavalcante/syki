using Syki.Database;
using Syki.Settings;
using Microsoft.EntityFrameworkCore;

namespace Syki.Configs;

public static class EfCoreConfigs
{
    public static void AddEfCoreConfigs(this IServiceCollection services)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        var db = services.BuildServiceProvider().GetService<DatabaseSettings>()!;

        services.AddDbContext<SykiDbContext>(options =>
        {
            options.UseNpgsql(db.ConnectionString);
            options.UseSnakeCaseNamingConvention();
        });
    }
}
