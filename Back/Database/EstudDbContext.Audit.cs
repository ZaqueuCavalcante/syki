using Estud.Back.Audit;

namespace Estud.Back.Database;

public partial class EstudDbContext
{
    private static void ConfigureAudit(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new AuditTrailDbConfig());
    }
}
