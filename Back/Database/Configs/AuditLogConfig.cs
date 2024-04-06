using Syki.Back.Audit;

namespace Syki.Back.Database;

public class AuditLogConfig : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> auditLog)
    {
        auditLog.ToTable("audit_logs");

        auditLog.HasKey(a => a.Id);
        auditLog.Property(a => a.Id).ValueGeneratedNever();

        auditLog.HasOne<SykiUser>()
            .WithMany()
            .HasPrincipalKey(u => new { u.InstitutionId, u.Id })
            .HasForeignKey(a => new { a.FaculdadeId, a.UserId });
    }
}
