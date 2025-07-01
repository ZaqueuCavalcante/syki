using Syki.Back.Storage;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Syki.Back.Configs;

public static class ServicesConfigs
{
    public static void AddServicesConfigs(this WebApplicationBuilder builder)
    {
        builder.Services.AddServices(typeof(IAdmService));
        builder.Services.AddServices(typeof(ICrossService));
        builder.Services.AddServices(typeof(IStudentService));
        builder.Services.AddServices(typeof(ITeacherService));
        builder.Services.AddServices(typeof(IAcademicService));

        builder.Services.AddScoped<IStorageService, AzureBlobStorageService>();

        if (Env.IsDevelopment() || Env.IsTesting())
        {
            builder.Services.Replace(ServiceDescriptor.Singleton<IStorageService, FakeStorageService>());
        }
    }

    private static void AddServices(this IServiceCollection services, Type marker)
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
