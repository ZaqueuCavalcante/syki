namespace Syki.Front.Configs;

public static class ServicesConfigs
{
    public static void AddServicesConfigs(this WebAssemblyHostBuilder builder)
    {
        builder.Services.AddClients(typeof(IAdmClient));
        builder.Services.AddClients(typeof(ICrossClient));
        builder.Services.AddClients(typeof(IStudentClient));
        builder.Services.AddClients(typeof(ITeacherClient));
        builder.Services.AddClients(typeof(IAcademicClient));

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
