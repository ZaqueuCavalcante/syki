using Syki.Back.Emails;
using Syki.Back.Configs;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Syki.Daemon.Configs;

public static class ServicesConfigs
{
    public static void AddDaemonServicesConfigs(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IEmailsService, EmailsService>();

        if (Env.IsDevelopment() || Env.IsTesting())
        {
            builder.Services.Replace(ServiceDescriptor.Singleton<IEmailsService, FakeEmailsService>());
        }

        builder.AddIdentityConfigs();

        builder.Services.AddHttpClient();
    }
}
