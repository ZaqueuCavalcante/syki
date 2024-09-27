using Syki.Back.Features.Cross.CreateUser;

namespace Syki.Back.Audit;

/// <summary>
/// Configuração da tabela de logs de auditoria.
/// </summary>
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
            .HasForeignKey(a => new { a.InstitutionId, a.UserId });
    }
}
