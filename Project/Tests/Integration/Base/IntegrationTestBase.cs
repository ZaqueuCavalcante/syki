using Exato.Web;
using Exato.Back.Events;
using Exato.Back.Commands;
using Exato.Tests.Integration.Seed;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Exato.Tests.Integration.Base;

public abstract class IntegrationTestBase
{
    protected BackFactory _back = null!;
    protected MocksFactory _mocks = null!;
    protected WorkersFactory _workers = null!;

    public static int ExatoId = 0;
    public static Guid ExatoExternalId = Guid.Empty;
    public const string ExatoAdmName = "Exato Adm";
    public const string ExatoAdmEmail = "adm@exato.com";
    public const string ExatoAdmPassword = "Exato.Adm@123";
    public static Guid ExatoAdminRoleId = Guid.Empty;

    public const string ExatoCSPassword = "Exato.CS@123";
    public static Guid ExatoCSRoleId = Guid.Empty;

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        EnvironmentExtensions.SetAsTesting();

        _back = new BackFactory();

        await ResetExatoDb();
        await ResetWebDb();

        _workers = new WorkersFactory();
        _workers.Services.CreateScope();

        _mocks = new MocksFactory();
        _mocks.StartServer();

        using var scope = _back.Services.CreateScope();
        var ctx = scope.ServiceProvider.GetRequiredService<BackDbContext>();

        await new PublicDataSeeder(ctx).Run();
        await new IbgeDataSeeder(ctx).Run();
        await new FaturamentoDataSeeder(ctx).Run();

        await _back.RegisterExatoAdm();
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        await _back.DisposeAsync();
        await _mocks.DisposeAsync();
        await _workers.DisposeAsync();
    }

    private async Task ResetExatoDb()
    {
        using var scope = _back.Services.CreateScope();
        var ctx = scope.ServiceProvider.GetRequiredService<BackDbContext>();

        if (ctx.HasMissingMigration()) throw new AssertionException("BackDbContext Has Missing Migration!");

        var cnn = ctx.Database.GetDbConnection().ConnectionString;

        if (!cnn.Contains("Host=localhost;")) throw new Exception("WRONG TESTS DB");

        await ctx.Database.EnsureDeletedAsync();
        await ctx.Database.MigrateAsync();
    }

    private async Task ResetWebDb()
    {
        using var scope = _back.Services.CreateScope();
        var ctx = scope.ServiceProvider.GetRequiredService<WebDbContext>();

        var cnn = ctx.Database.GetDbConnection().ConnectionString;

        if (!cnn.Contains("Host=localhost;")) throw new Exception("WRONG TESTS DB");

        await ctx.Database.EnsureDeletedAsync();
        await ctx.Database.EnsureCreatedAsync();
    }

    protected async Task AssertSingleDomainEvent<T>(string like) where T : IDomainEvent
    {
        using var ctx = _back.GetBackDbContext();

        var events = await ctx.ExatoDomainEvents.AsNoTracking().Where(x => x.Data.Contains(like)).ToListAsync();

        events.Where(x => typeof(Command).Assembly.GetType(x.Type) == typeof(T)).Should().ContainSingle();
    }
}
