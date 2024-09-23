namespace Syki.Back.Configs;

public static class ServicesConfigs
{
    public static void AddServicesConfigs(this IServiceCollection services)
    {
        services.AddServiceConfigs(typeof(IAcademicService));
        services.AddServiceConfigs(typeof(IAdmService));
        services.AddServiceConfigs(typeof(ICrossService));
        services.AddServiceConfigs(typeof(IStudentService));
        services.AddServiceConfigs(typeof(ITeacherService));
    }

    private static void AddServiceConfigs(this IServiceCollection services, Type marker)
    {
        var types = AppDomain.CurrentDomain.GetAssemblies()
            .Where(s => s.FullName.StartsWith("Back"))
            .SelectMany(s => s.GetTypes())
            .Where(p => marker.IsAssignableFrom(p) && !p.IsInterface)
            .ToList();

        foreach (var type in types)
        {
            services.AddScoped(type);
        }
    }
}
