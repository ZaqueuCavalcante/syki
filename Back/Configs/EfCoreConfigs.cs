namespace Syki.Back.Configs;

public static class EfCoreConfigs
{
    public static void AddEfCoreConfigs(this WebApplicationBuilder builder)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        builder.Services.AddDbContext<SykiDbContext>();
    }
}
