namespace Syki.Front.Configs;

public static class ServicesConfigs
{
    public static void AddServicesConfigs(this WebAssemblyHostBuilder builder)
    {
        builder.Services.AddServiceConfigs(typeof(IAcademicClient));
        builder.Services.AddServiceConfigs(typeof(IAdmClient));
        builder.Services.AddServiceConfigs(typeof(ICrossClient));
        builder.Services.AddServiceConfigs(typeof(ISellerClient));
        builder.Services.AddServiceConfigs(typeof(IStudentClient));
        builder.Services.AddServiceConfigs(typeof(ITeacherClient));
    }

    private static void AddServiceConfigs(this IServiceCollection services, Type? marker)
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
