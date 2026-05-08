using Syki.Back.Commands.Domain.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace Syki.Tests.Base;

public abstract class IntegrationTestBase
{
    protected BackFactory _api = null!;

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        Env.SetAsTesting();

        _api = new BackFactory();
        using var scope = _api.Services.CreateScope();
        var ctx = scope.ServiceProvider.GetRequiredService<SykiDbContext>();

        if (ctx.HasMissingMigration()) throw new AssertionException("Missing Migration!");

        await ctx.ResetTestDbAsync();
        await _api.RegisterAdm();
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        await _api.DisposeAsync();
    }

    protected async Task AssertCommand<T>(string like) where T : ICommand
    {
        using var ctx = _api.GetDbContext();

        var commands = await ctx.Commands.Where(x => x.Data.Contains(like)).ToListAsync();

        commands.Should().ContainSingle();
        typeof(Command).Assembly.GetType(commands[0].Type).Should().Be<T>();
    }
}
