namespace Syki.Back.Features.Cross.LoadFeatureFlags;

public class LoadFeatureFlagsHostedService(IServiceScopeFactory serviceScopeFactory, FeaturesSettings settings) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        if (Env.IsTesting()) return;

        await Task.Delay(10_000, cancellationToken);

        using IServiceScope scope = serviceScopeFactory.CreateScope();
        var ctx = scope.ServiceProvider.GetRequiredService<SykiDbContext>();

        var features = await ctx.FeatureFlags.AsNoTracking()
            .Where(f => f.Id == Guid.Empty)
            .FirstOrDefaultAsync(cancellationToken);

        if (features == null) return;

        settings.CrossLogin = features.CrossLogin;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
