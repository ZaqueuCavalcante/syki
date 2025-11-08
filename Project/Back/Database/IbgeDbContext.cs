using Exato.Back.Intelligence.Domain.Ibge;
using Exato.Back.Intelligence.Configs.Ibge;

namespace Exato.Back.Database;

public partial class BackDbContext
{
    public DbSet<CnaeConsolidado> IbgeCnaeConsolidado { get; set; }

    private static void ConfigureIbgeSchema(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CnaeConsolidadoDbConfig());
    }
}
