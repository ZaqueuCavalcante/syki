using Quartz;
using Estud.Back.Emails;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Estud.Tests.Integration.Clients;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Estud.Tests.Base;

extern alias Back;

public class BackFactory : WebApplicationFactory<Back::Program>
{
    public BackFactory() : base()
    {
        UseKestrel(o => o.ListenLocalhost(5100));
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        EnvironmentExtensions.SetAsTesting();

        builder.UseTestServer();

        builder.ConfigureAppConfiguration(config =>
        {
            var configPath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.Testing.json");

            var configuration = new ConfigurationBuilder()
                .AddJsonFile(configPath)
                .Build();

            config.AddConfiguration(configuration);
        });
    }

    public TestsHttpClient GetTestsClient()
    {
        return new TestsHttpClient(CreateClient());
    }

    public EstudDbContext GetDbContext()
    {
        var scope = Services.CreateScope();
        return scope.ServiceProvider.GetRequiredService<EstudDbContext>();
    }

    public ISchedulerFactory GetSchedulerFactory()
    {
        var scope = Services.CreateScope();
        return scope.ServiceProvider.GetRequiredService<ISchedulerFactory>();
    }

    public FakeEmailsService GetFakeEmailsService()
    {
        return (FakeEmailsService)Services.GetRequiredService<IEmailsService>();
    }
}
