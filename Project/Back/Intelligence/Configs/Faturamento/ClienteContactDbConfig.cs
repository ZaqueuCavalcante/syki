using Exato.Back.Intelligence.Domain.Public;
using Exato.Back.Intelligence.Domain.Faturamento;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Exato.Back.Intelligence.Configs.Faturamento;

public class ClienteContactDbConfig : IEntityTypeConfiguration<ClienteContact>
{
    public void Configure(EntityTypeBuilder<ClienteContact> entity)
    {
        entity.ToTable("cliente_contact", "faturamento");

        entity.HasKey(e => e.Id)
            .HasName("cliente_contact_pkey");

        entity.Property(e => e.Id)
            .UseSerialColumn()
            .HasColumnName("id");

        entity.Property(e => e.Active)
            .HasDefaultValue(true)
            .HasColumnName("active");

        entity.Property(e => e.AdditionalData)
            .HasColumnName("additional_data");

        entity.Property(e => e.ClienteId)
            .HasColumnName("cliente_id");

        entity.Property(e => e.Email)
            .HasColumnName("email");

        entity.Property(e => e.InsertedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .HasColumnType("timestamp")
            .HasColumnName("inserted_at");

        entity.Property(e => e.Name)
            .HasColumnName("name");

        entity.HasOne<Cliente>()
            .WithMany()
            .HasPrincipalKey(c => c.ClienteId)
            .HasForeignKey(e => e.ClienteId);
    }
}
