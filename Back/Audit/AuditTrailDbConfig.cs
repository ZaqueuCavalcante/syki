namespace Estud.Back.Audit;

public class AuditTrailDbConfig : IEntityTypeConfiguration<AuditTrail>
{
    public void Configure(EntityTypeBuilder<AuditTrail> entity)
    {
        entity.ToTable("audit_trails", DbSchemas.Estud);

        entity.HasKey(e => e.Id);

        entity.Property(e => e.Data).HasColumnType("jsonb");
    }
}
