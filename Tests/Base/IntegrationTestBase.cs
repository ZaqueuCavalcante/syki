using Npgsql;
using Estud.Tests.Seed;
using Estud.Back.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Estud.Tests.Base;

public abstract class IntegrationTestBase
{
    protected BackFactory _back = null!;
    protected MocksFactory _mocks = null!;

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        EnvironmentExtensions.SetAsTesting();

        await ResetEstudDb();

        _mocks = new MocksFactory();
        _mocks.StartServer();

        _back = new BackFactory();
        using var scope = _back.Services.CreateScope();
        var ctx = scope.ServiceProvider.GetRequiredService<EstudDbContext>();

        await new DataSeeder(ctx).Run();
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        await _back.DisposeAsync();
        await _mocks.DisposeAsync();
    }

    private static async Task ResetEstudDb()
    {
        var configPath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.Testing.json");
        var configuration = new ConfigurationBuilder().AddJsonFile(configPath).Build();
        var connectionString = configuration.Database.ConnectionString;

        var dataSource = new NpgsqlDataSourceBuilder(connectionString).Build();
        var options = new DbContextOptionsBuilder<EstudDbContext>().Options;

        using var ctx = new EstudDbContext(options, dataSource, null);
        // if (ctx.HasMissingMigration()) throw new AssertionException("EstudDbContext Has Missing Migration!");

        var cnn = ctx.Database.GetDbConnection().ConnectionString;
        if (!cnn.Contains("Host=localhost;")) throw new Exception("WRONG TESTS DB");

        await ctx.Database.EnsureDeletedAsync();
        await ctx.Database.EnsureCreatedAsync();
    }
}
