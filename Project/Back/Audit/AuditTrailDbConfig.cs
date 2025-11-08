using Exato.Back.Features.Cross.CreateExatoUser;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Exato.Back.Audit;

public class AuditTrailDbConfig : IEntityTypeConfiguration<AuditTrail>
{
    public void Configure(EntityTypeBuilder<AuditTrail> entity)
    {
        entity.ToTable("audit_trails", "exato");

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).ValueGeneratedNever();

        entity.Property(e => e.CreatedAt)
            .HasColumnType("timestamp");
    }
}
