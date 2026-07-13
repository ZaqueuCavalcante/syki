using Estud.Back.Emails;
using Estud.Back.Google;
using Estud.Back.Storage;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Estud.Back.Configs;

public static class ServicesConfigs
{
    public static void AddServicesConfigs(this WebApplicationBuilder builder)
    {
        builder.Services.AddServices(typeof(IEstudService));

        builder.Services.AddScoped<IEmailsService, EmailsService>();
        builder.Services.AddScoped<IGoogleService, GoogleService>();
        builder.Services.AddScoped<IStorageService, FakeStorageService>();

        if (EnvironmentExtensions.IsTesting())
        {
            builder.Services.Replace(ServiceDescriptor.Singleton<IGoogleService, FakeGoogleService>());
            builder.Services.Replace(ServiceDescriptor.Singleton<IEmailsService, FakeEmailsService>());
        }
        if (EnvironmentExtensions.IsDevelopment())
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
