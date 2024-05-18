using Microsoft.Extensions.DependencyInjection;

namespace Syki.Tests.Base;

public class IntegrationTestBase
{
    protected BackWebApplicationFactory _factory = null!;

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        _factory = new BackWebApplicationFactory();
        using var scope = _factory.Services.CreateScope();
        var ctx = scope.ServiceProvider.GetRequiredService<SykiDbContext>();

        await ctx.ResetDbAsync();

        await _factory.RegisterAdm();
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        await _factory.DisposeAsync();
    }

    protected async Task AssertTaskByDataLike<T>(string like)
    {
        using var ctx = _factory.GetDbContext();

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
