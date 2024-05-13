using Syki.Back.Configs;
using Syki.Daemon.Emails;
using Syki.Back.Extensions;
using Syki.Back.Features.Academic.CreateTeacher;
using Syki.Back.Features.Cross.CreateUser;
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
            services.AddScoped<CreateTeacherService>();
            services.AddScoped<SendResetPasswordTokenService>();

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
