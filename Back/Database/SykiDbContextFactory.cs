using Npgsql;
using Microsoft.EntityFrameworkCore.Design;

namespace Syki.Back.Database;

public class SykiDbContextFactory : IDesignTimeDbContextFactory<SykiDbContext>
{
    public SykiDbContext CreateDbContext(string[] args)
    {
        return new SykiDbContext(new(), NpgsqlDataSource.Create(GetAppConfiguration().Database().ConnectionString));
    }

    static IConfiguration GetAppConfiguration()
    {
        var builder = new ConfigurationBuilder().AddEnvironmentVariables();
        return builder.Build();
    }
}
