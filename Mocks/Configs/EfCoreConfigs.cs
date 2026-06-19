using Syki.Back.Database;

namespace Syki.Mocks.Configs;

public static class EfCoreConfigs
{
    public static void AddEfCoreConfigs(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<SykiDbContext>();
    }
}
