namespace Exato.Front.Configs;

public static class ServicesConfigs
{
    public static void AddServicesConfigs(this WebAssemblyHostBuilder builder)
    {
        builder.Services.AddClients(typeof(ICrossClient));
        builder.Services.AddClients(typeof(IOrgsClient));
        builder.Services.AddClients(typeof(IOfficeClient));

        builder.Services.AddScoped<ClipboardService>();
    }

    private static void AddClients(this IServiceCollection services, Type? marker)
    {
        var types = AppDomain.CurrentDomain.GetAssemblies()
            .Where(s => s.FullName.StartsWith("Front"))
            .SelectMany(s => s.GetTypes())
            .Where(p => marker.IsAssignableFrom(p) && !p.IsInterface)
            .ToList();

        foreach (var type in types)
        {
            services.AddScoped(type);
        }
    }
}
