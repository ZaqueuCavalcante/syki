using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Syki.Tests.Base;

extern alias Back;

public class BackFactory : WebApplicationFactory<Back::Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        Env.SetAsTesting();

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

	private readonly static TimeSpan ShutdownTimeout = TimeSpan.FromSeconds(60);

	public override async ValueTask DisposeAsync()
	{
		await StopApplication().ConfigureAwait(false);

		foreach (var factory in Factories)
		{
			await factory.DisposeAsync().ConfigureAwait(false);
		}
	}

	private async Task StopApplication(CancellationToken forcefulStoppingToken = default)
	{
		try
		{
			var tcs = new TaskCompletionSource();
			var lifetime = Services.GetRequiredService<IHostApplicationLifetime>();
			lifetime.ApplicationStopped.Register(() => tcs.TrySetResult());
			lifetime.StopApplication();
			await tcs.Task.WaitAsync(ShutdownTimeout, forcefulStoppingToken).ConfigureAwait(false);
		}
		catch (Exception)
		{
		}
	}
}
