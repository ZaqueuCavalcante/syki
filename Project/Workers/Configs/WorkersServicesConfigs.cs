using Exato.Back.Emails;
using Exato.Shared.Extensions;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Exato.Workers.Configs;

public static class WorkersServicesConfigs
{
    public static void AddWorkersServicesConfigs(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IEmailService, SendGridEmailService>();

        if (EnvironmentExtensions.IsDevelopment() || EnvironmentExtensions.IsTesting())
        {
            builder.Services.Replace(ServiceDescriptor.Singleton<IEmailService, FakeEmailService>());
        }
    }
}
