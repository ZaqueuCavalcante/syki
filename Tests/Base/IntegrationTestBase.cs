using Microsoft.Extensions.DependencyInjection;

namespace Syki.Tests.Base;

public class IntegrationTestBase
{
    protected BackWebApplicationFactory _back = null!;
    protected DaemonWebApplicationFactory _daemon = null!;

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        _back = new BackWebApplicationFactory();
        _daemon = new DaemonWebApplicationFactory();
        using var scope = _back.Services.CreateScope();
        var ctx = scope.ServiceProvider.GetRequiredService<SykiDbContext>();

        await ctx.ResetDbAsync();
        _daemon.Services.CreateScope();

        await _back.RegisterAdm();
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        await _back.DisposeAsync();
        await _daemon.DisposeAsync();
    }

    protected async Task AssertTaskByDataLike<T>(string like)
    {
        using var ctx = _back.GetDbContext();

        var likeFormat = $"%{like}%";
        FormattableString sql = $@"
            SELECT *
            FROM syki.tasks
            WHERE data LIKE {likeFormat}
        ";

        var tasks = await ctx.Database.SqlQuery<SykiTask>(sql).ToListAsync();

        tasks.Should().ContainSingle();
        typeof(SykiTask).Assembly.GetType(tasks[0].Type).Should().Be<T>();
    }
}
