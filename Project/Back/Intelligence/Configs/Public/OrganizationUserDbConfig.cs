using Exato.Back.Intelligence.Domain.Public;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Exato.Back.Intelligence.Configs.Public;

public class OrganizationUserDbConfig : IEntityTypeConfiguration<OrganizationUser>
{
    public void Configure(EntityTypeBuilder<OrganizationUser> entity)
    {
        entity.ToTable("organization_users", "public");

        entity.HasKey(e => e.Id)
            .HasName("organization_users_pk");

        entity.Property(e => e.Id)
            .UseSerialColumn()
            .HasColumnName("id");

        entity.Property(e => e.ClienteId)
            .HasColumnName("cliente_id");

        entity.Property(e => e.CreatedAt)
            .HasColumnType("timestamp")
            .HasColumnName("created_at");

        entity.Property(e => e.CreatedBy)
            .HasColumnName("created_by");

        entity.Property(e => e.IndicationCode)
            .HasMaxLength(12)
            .HasColumnName("indication_code");

        entity.Property(e => e.ItsHisOwn)
            .HasDefaultValue(false)
            .HasColumnName("its_his_own");

        entity.Property(e => e.JoinedAt)
            .HasColumnType("timestamp")
            .HasColumnName("joined_at");

        entity.Property(e => e.LeavedAt)
            .HasColumnType("timestamp")
            .HasColumnName("leaved_at");

        entity.Property(e => e.UserId)
            .HasColumnName("user_id");

        entity.HasIndex(e => new { e.ClienteId, e.UserId, e.ItsHisOwn })
            .HasDatabaseName("organization_users_cliente_user_fkey");
    }
}
