using Syki.Back;
using Syki.Back.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace Syki.Tests.Base;

public class SykiWebApplicationFactory : WebApplicationFactory<Startup>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        Env.SetAsTesting();

        builder.ConfigureAppConfiguration(config =>
        {
            var configPath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.Testing.json");

            var configuration = new ConfigurationBuilder()
                .AddJsonFile(configPath)
                .Build();

            config.AddConfiguration(configuration);
        });
    }
}
