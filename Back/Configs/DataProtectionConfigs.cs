using Microsoft.AspNetCore.DataProtection;

namespace Syki.Back.Configs;

public static class DataProtectionConfigs
{
    public static void AddDataProtectionConfigs(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddDataProtection()
            .SetApplicationName("Syki")
            .PersistKeysToDbContext<SykiDbContext>();
    }
}
