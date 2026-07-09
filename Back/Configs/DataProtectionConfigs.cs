using Microsoft.AspNetCore.DataProtection;

namespace Estud.Back.Configs;

public static class DataProtectionConfigs
{
    public static void AddDataProtectionConfigs(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddDataProtection()
            .SetApplicationName("Estud")
            .PersistKeysToDbContext<EstudDbContext>();
    }
}
