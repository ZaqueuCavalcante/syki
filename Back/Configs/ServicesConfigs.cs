using Syki.Back.Emails;
using Syki.Back.Storage;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Syki.Back.Configs;

public static class ServicesConfigs
{
    public static void AddServicesConfigs(this WebApplicationBuilder builder)
    {
        builder.Services.AddServices(typeof(ISykiService));

        builder.Services.AddScoped<IEmailsService, EmailsService>();
        builder.Services.AddScoped<IStorageService, FakeStorageService>();

        if (EnvironmentExtensions.IsDevelopmentOrTesting())
        {
            builder.Services.Replace(ServiceDescriptor.Singleton<IEmailsService, FakeEmailsService>());
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
