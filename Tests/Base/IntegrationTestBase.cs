using Microsoft.Extensions.DependencyInjection;

namespace Syki.Tests.Base;

public abstract class IntegrationTestBase
{
    protected BackFactory _back = null!;

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        Env.SetAsTesting();

        _back = new BackFactory();
        using var scope = _back.Services.CreateScope();
        var ctx = scope.ServiceProvider.GetRequiredService<SykiDbContext>();

        if (ctx.HasMissingMigration()) throw new AssertionException("Missing Migration!");

        await ctx.ResetTestDbAsync();
        await _back.RegisterAdm();
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        await _back.DisposeAsync();
    }
}
