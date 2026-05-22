using Microsoft.EntityFrameworkCore.Design;
using Npgsql;

namespace Syki.Back.Database;

public class SykiDbContextFactory : IDesignTimeDbContextFactory<SykiDbContext>
{
    public SykiDbContext CreateDbContext(string[] args)
    {
        var connectionString = "Host=localhost;Database=syki;Username=postgres;Password=postgres";

        var options = new DbContextOptionsBuilder<SykiDbContext>()
            .UseNpgsql(connectionString)
            .UseSnakeCaseNamingConvention()
            .Options;

        var dataSource = new NpgsqlDataSourceBuilder(connectionString).Build();

        return new SykiDbContext(options, dataSource, null!);
    }
}
