using Audit.Core;
using Syki.Back.Configs;
using Syki.Daemon.Emails;
using Syki.Daemon.Configs;
using Syki.Back.CreateUser;
using Syki.Back.Extensions;
using Syki.Back.CreateProfessor;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

var builder = Host.CreateDefaultBuilder(args);

Configuration.AuditDisabled = true;

builder.ConfigureAppConfiguration(config =>
{
    var configPath = Path.Combine(Directory.GetCurrentDirectory(), $"appsettings.{Env.Get()}.json");

    var configuration = new ConfigurationBuilder()
        .AddJsonFile(configPath)
        .Build();

    config.AddConfiguration(configuration);
});

builder.ConfigureServices((ctx, services) =>
{
    services.AddEfCoreConfigs();

    services.AddScoped<CreateUserService>();
    services.AddScoped<CreateProfessorService>();

    services.AddScoped<IEmailsService, EmailsService>();
    if (Env.IsDevelopment())
    {
        services.Replace(ServiceDescriptor.Scoped<IEmailsService, FakeEmailsService>());
    }

    services.AddIdentityConfigs();
    services.AddSykiTasksConfigs();
});

using var host = builder.Build();

await host.RunAsync();
