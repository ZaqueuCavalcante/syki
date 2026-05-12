using Syki.Back.Audit;
namespace Syki.Back.Database;

public partial class SykiDbContext
{
    private static void ConfigureAudit(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new AuditTrailDbConfig());
    }
}
