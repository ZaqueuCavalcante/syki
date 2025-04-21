using Microsoft.EntityFrameworkCore.Design;

namespace Syki.Back.Database;

public class SykiDbContextFactory : IDesignTimeDbContextFactory<SykiDbContext>
{
    public SykiDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<SykiDbContext>();
        var settings = new DatabaseSettings() { ConnectionString = args.First() };

        return new SykiDbContext(optionsBuilder.Options, settings);
    }
}
