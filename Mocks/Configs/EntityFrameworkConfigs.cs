using Syki.Back.Database;

namespace Syki.Mocks.Configs;

public static class EntityFrameworkConfigs
{
    public static void AddEntityFrameworkConfigs(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<SykiDbContext>();
    }
}
