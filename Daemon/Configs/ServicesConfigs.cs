using Syki.Back.Configs;
using Syki.Daemon.Emails;
using Syki.Back.Extensions;
using Syki.Back.CreateProfessor;
using Microsoft.Extensions.Hosting;
using Syki.Back.Features.Cross.CreateUser;
using Microsoft.Extensions.DependencyInjection;
using Syki.Back.Features.Cross.SendResetPasswordToken;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Syki.Daemon.Configs;

public static class ServicesConfigs
{
    public static void AddServicesConfigs(this IHostBuilder builder)
    {
        builder.ConfigureServices((ctx, services) =>
        {
            services.AddEfCoreConfigs();

            services.AddScoped<CreateUserService>();
            services.AddScoped<CreateProfessorService>();
            services.AddScoped<SendResetPasswordEmailService>();

            services.AddScoped<IEmailsService, EmailsService>();
            if (Env.IsDevelopment())
            {
                services.Replace(ServiceDescriptor.Scoped<IEmailsService, FakeEmailsService>());
            }

            services.AddIdentityConfigs();
            services.AddSykiTasksConfigs();
        });
    }
}
