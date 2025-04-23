namespace Syki.Back.Configs;

public static class EfCoreConfigs
{
    public static void AddEfCoreConfigs(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<SykiDbContext>();
    }
}
