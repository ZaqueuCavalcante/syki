using Npgsql;
using Syki.Tests.Seed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Syki.Tests.Base;

public abstract class IntegrationTestBase
{
    protected BackFactory _back = null!;

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        EnvironmentExtensions.SetAsTesting();

        await ResetSykiDb();

        _back = new BackFactory();
        using var scope = _back.Services.CreateScope();
        var ctx = scope.ServiceProvider.GetRequiredService<SykiDbContext>();

        await new DataSeeder(ctx).Run();
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        await _back.DisposeAsync();
    }

    private static async Task ResetSykiDb()
    {
        var configPath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.Testing.json");
        var configuration = new ConfigurationBuilder().AddJsonFile(configPath).Build();
        var connectionString = configuration["Database:ConnectionString"];

        var dataSource = new NpgsqlDataSourceBuilder(connectionString).Build();
        var options = new DbContextOptionsBuilder<SykiDbContext>().Options;

        using var ctx = new SykiDbContext(options, dataSource, null);

        if (ctx.HasMissingMigration()) throw new AssertionException("SykiDbContext Has Missing Migration!");

        var cnn = ctx.Database.GetDbConnection().ConnectionString;

        if (!cnn.Contains("Host=localhost;")) throw new Exception("WRONG TESTS DB");

        await ctx.Database.EnsureDeletedAsync();
        await ctx.Database.MigrateAsync();
    }
}
