using Syki.Back.Database;
using Syki.Back.Settings;
using Microsoft.EntityFrameworkCore;

namespace Syki.Back.Configs;

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
