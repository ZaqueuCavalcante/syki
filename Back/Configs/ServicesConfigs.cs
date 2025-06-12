using Syki.Back.Storage;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Syki.Back.Configs;

public static class ServicesConfigs
{
    public static void AddServicesConfigs(this WebApplicationBuilder builder)
    {
        builder.Services.AddServiceConfigs(typeof(IAcademicService));
        builder.Services.AddServiceConfigs(typeof(IAdmService));
        builder.Services.AddServiceConfigs(typeof(ICrossService));
        builder.Services.AddServiceConfigs(typeof(IStudentService));
        builder.Services.AddServiceConfigs(typeof(ITeacherService));

        builder.Services.AddScoped<IStorageService, AzureBlobStorageService>();
        if (Env.IsTesting())
        {
            builder.Services.Replace(ServiceDescriptor.Singleton<IStorageService, FakeStorageService>());
        }
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
