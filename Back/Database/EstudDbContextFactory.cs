using Npgsql;
using Microsoft.EntityFrameworkCore.Design;

namespace Estud.Back.Database;

public class EstudDbContextFactory : IDesignTimeDbContextFactory<EstudDbContext>
{
    public EstudDbContext CreateDbContext(string[] args)
    {
        var connectionString = "Host=localhost;Database=estud;Username=postgres;Password=postgres";

        var options = new DbContextOptionsBuilder<EstudDbContext>()
            .UseNpgsql(connectionString)
            .UseSnakeCaseNamingConvention()
            .Options;

        var dataSource = new NpgsqlDataSourceBuilder(connectionString).Build();

        return new EstudDbContext(options, dataSource, null!);
    }
}
