namespace Syki.Back.Metrics;

public class SetupCurrentMetricsStore(IServiceScopeFactory serviceScopeFactory) : IHostedService
{
    private RequestMetrics _requestMetrics = new();

    public Task StartAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        if (!Env.IsTesting()) return;

        using IServiceScope scope = serviceScopeFactory.CreateScope();
        var ctx = scope.ServiceProvider.GetRequiredService<SykiDbContext>();

        var requests = SykiMetricsStore.Requests
            .Select(x => new RequestData { Endpoint = x.Key, Values = x.Value })
            .OrderByDescending(x => x.Values["Total"])
            .ToList();

        _requestMetrics.Save(requests);

        try
        {
            ctx.Add(_requestMetrics);
            await ctx.SaveChangesAsync(cancellationToken);
        }
        catch (Exception) { }
    }
}
