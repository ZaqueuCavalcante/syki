using Microsoft.Extensions.DependencyInjection;

namespace Syki.Tests.Base;

public class IntegrationTestBase
{
    protected BackFactory _api = null!;
    protected DaemonFactory _daemon = null!;

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        _api = new BackFactory();
        using var scope = _api.Services.CreateScope();
        var ctx = scope.ServiceProvider.GetRequiredService<SykiDbContext>();

        await ctx.ResetDbAsync();
        await _api.RegisterAdm();

        _daemon = new DaemonFactory();
        _daemon.Services.CreateScope();
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        if (_api != null) await _api.DisposeAsync();
        if (_daemon != null) await _daemon.DisposeAsync();
    }

    protected async Task AssertTaskByDataLike<T>(string like)
    {
        using var ctx = _api.GetDbContext();

        var tasks = await ctx.Tasks.Where(x => x.Data.Contains(like)).ToListAsync();

        tasks.Should().ContainSingle();
        typeof(SykiTask).Assembly.GetType(tasks[0].Type).Should().Be<T>();
    }
}
