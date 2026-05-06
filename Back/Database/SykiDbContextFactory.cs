using Npgsql;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace Syki.Back.Database;

public class SykiDbContextFactory : IDesignTimeDbContextFactory<SykiDbContext>
{
    public SykiDbContext CreateDbContext(string[] args)
    {
        var environment =
            Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ??
            Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ??
            "Development";

        var basePath = Directory.GetCurrentDirectory();

        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile($"appsettings.{environment}.json", optional: true)
            .AddUserSecrets(Assembly.GetExecutingAssembly(), optional: true)
            .AddEnvironmentVariables()
            .Build();

        var connectionString = configuration.GetSection("Database")["ConnectionString"]
            ?? "UserID=postgres;Password=postgres;Host=localhost;Port=5432;Database=syki-db;Pooling=true;";

        var dataSource = NpgsqlDataSource.Create(connectionString);

        return new SykiDbContext(new(), dataSource);
    }
}
