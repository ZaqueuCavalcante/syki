namespace Syki.Back.Settings;

public class FeaturesSettings
{
    public bool SkipUserRegister { get; set; }
    public bool CrossLogin { get; set; }

    public FeaturesSettings() {}

    public FeaturesSettings(IConfiguration configuration)
    {
        configuration.GetSection("Features").Bind(this);
    }
}

public class LoadFeatureFlagsFromDb(IServiceScopeFactory serviceScopeFactory, FeaturesSettings settings) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        if (Env.IsTesting()) return;

        using IServiceScope scope = serviceScopeFactory.CreateScope();
        var ctx = scope.ServiceProvider.GetRequiredService<SykiDbContext>();

        var features = await ctx.FeatureFlags.AsNoTracking()
            .Where(f => f.Id == Guid.Empty)
            .FirstOrDefaultAsync(cancellationToken);

        if (features == null) return;

        settings.SkipUserRegister = features.Settings.SkipUserRegister;
        settings.CrossLogin = features.Settings.CrossLogin;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
