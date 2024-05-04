namespace Syki.Back.Configs;

public static class EfCoreConfigs
{
    public static void AddEfCoreConfigs(this IServiceCollection services)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        services.AddDbContext<SykiDbContext>();
    }
}
