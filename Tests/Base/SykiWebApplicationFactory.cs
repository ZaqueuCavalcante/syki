using Syki.Back;
using Audit.Core;
using Syki.Back.Extensions;
using Testcontainers.PostgreSql;
using Microsoft.AspNetCore.Hosting;
using DotNet.Testcontainers.Builders;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace Syki.Tests.Base;

public class SykiWebApplicationFactory : WebApplicationFactory<Startup>
{
    private readonly PostgreSqlContainer _postgreSqlContainer = new PostgreSqlBuilder()
        .WithName("tests-postgres")
        .WithImage("postgres:latest")
        .WithEnvironment("POSTGRES_DB", "syki-tests-db")
        .WithEnvironment("POSTGRES_USER", "postgres")
        .WithEnvironment("POSTGRES_PASSWORD", "postgres")
        .WithPortBinding(5432, 5432)
        .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(5432))
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        Env.SetAsTesting();
        Configuration.AuditDisabled = true;

        builder.ConfigureAppConfiguration(config =>
        {
            var configPath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.Testing.json");

            var configuration = new ConfigurationBuilder()
                .AddJsonFile(configPath)
                .Build();

            config.AddConfiguration(configuration);
        });

        _postgreSqlContainer.StartAsync().GetAwaiter().GetResult();
    }
}
