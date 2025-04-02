using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting.Server.Features;

namespace Syki.Tests.Base;

extern alias Server;

public sealed class FrontFactory : WebApplicationFactory<Server::Program>
{
    private bool _disposed;
    private IHost? _kestrelServerHost;

    public string ServerAddress
    {
        get
        {
            EnsureServer();
            return ClientOptions.BaseAddress.ToString();
        }
    }

    public override IServiceProvider Services
    {
        get
        {
            EnsureServer();
            return _kestrelServerHost!.Services;
        }
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);

        builder.UseUrls("http://127.0.0.1:0");
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        var testServerHost = builder.Build();

        builder.ConfigureWebHost(p => p.UseKestrel().UseUrls("http://*:5002"));

        _kestrelServerHost = builder.Build();
        _kestrelServerHost.Start();

        var server = _kestrelServerHost.Services.GetRequiredService<IServer>();
        var addresses = server.Features.Get<IServerAddressesFeature>();

        ClientOptions.BaseAddress = addresses!.Addresses
            .Select(p => new Uri(p))
            .Last();

        testServerHost.Start();

        return testServerHost;
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        if (!_disposed)
        {
            if (disposing)
            {
                _kestrelServerHost?.Dispose();
            }
            _disposed = true;
        }
    }

    private void EnsureServer()
    {
        using var _ = CreateDefaultClient();
    }
}
