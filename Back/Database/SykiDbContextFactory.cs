using Npgsql;
using Microsoft.EntityFrameworkCore.Design;

namespace Syki.Back.Database;

public class SykiDbContextFactory : IDesignTimeDbContextFactory<SykiDbContext>
{
    public SykiDbContext CreateDbContext(string[] args)
    {
        return new SykiDbContext(new(), NpgsqlDataSource.Create("UserID=a;Password=b;Host=c;Port=5432;Database=d;"));
    }
}
