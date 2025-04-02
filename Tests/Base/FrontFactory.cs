using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Connections;

namespace Syki.Tests.Base;

public sealed class FrontFactory : WebApplicationFactory<Command>
{
    public int Port { get; set; } = 5002;

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureWebHost(webHostBuilder =>
        {
            webHostBuilder.UseKestrel(opt => opt.ListenLocalhost(Port));
            webHostBuilder.ConfigureServices(s => s.AddSingleton<IServer, KestrelTestServer>());
        });

        var host = base.CreateHost(builder);

        return host;
    }

    public override async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();
        GC.SuppressFinalize(this);
    }
}

public class KestrelTestServer : TestServer, IServer
{
    private readonly KestrelServer _server;

    public KestrelTestServer(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        var transportFactory = serviceProvider.GetRequiredService<IEnumerable<IConnectionListenerFactory>>().First();

        var kestrelOptions = serviceProvider.GetRequiredService<IOptions<KestrelServerOptions>>();
        var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
        _server = new KestrelServer(kestrelOptions, transportFactory, loggerFactory);
    }

    async Task IServer.StartAsync<TContext>(IHttpApplication<TContext> application, CancellationToken cancellationToken)
    {
        await InvokeExplicitInterfaceMethod(nameof(IServer.StartAsync), typeof(TContext), [application, cancellationToken]);
        await _server.StartAsync(application, cancellationToken);
    }

    async Task IServer.StopAsync(CancellationToken cancellationToken)
    {
        await InvokeExplicitInterfaceMethod(nameof(IServer.StopAsync), null, [cancellationToken]);
        await _server.StopAsync(cancellationToken);
    }

    private Task InvokeExplicitInterfaceMethod(string methodName, Type? genericParameter, object[] args)
    {
        var baseMethod = typeof(TestServer).GetInterfaceMap(typeof(IServer)).TargetMethods.First(m => m.Name.EndsWith(methodName));
        var method = genericParameter == null ? baseMethod : baseMethod.MakeGenericMethod(genericParameter);
        return method.Invoke(this, args) as Task ?? throw new InvalidOperationException("Task not returned");
    }
}
