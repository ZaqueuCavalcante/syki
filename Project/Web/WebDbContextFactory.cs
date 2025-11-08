using Microsoft.EntityFrameworkCore.Design;

namespace Exato.Web;

public class WebDbContextFactory : IDesignTimeDbContextFactory<WebDbContext>
{
    public WebDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<WebDbContext>();
        optionsBuilder.UseNpgsql("UserID=a;Password=b;Host=c;Port=5432;Database=d;"); 
        return new WebDbContext(optionsBuilder.Options);
    }
}
