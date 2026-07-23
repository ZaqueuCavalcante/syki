using Estud.Back.Domain.DomainEvents;
using Estud.Back.Database.DomainEvents;

namespace Estud.Back.Database;

public partial class EstudDbContext
{
    public bool HasPendingDomainEvents { get; set; }

    public DbSet<DomainEvent> DomainEvents { get; set; }

    private static void ConfigureDomainEvents(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new DomainEventDbConfig());
    }
}
