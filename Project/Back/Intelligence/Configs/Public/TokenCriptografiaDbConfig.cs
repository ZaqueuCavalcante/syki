using Exato.Back.Intelligence.Domain.Public;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Exato.Back.Intelligence.Configs.Public;

public class TokenCriptografiaDbConfig : IEntityTypeConfiguration<TokenCriptografia>
{
    public void Configure(EntityTypeBuilder<TokenCriptografia> entity)
    {
        entity.ToTable("token_criptografia", "public");

        entity.HasKey(e => e.TokenCriptografiaId)
            .HasName("token_criptografia_pkey");

        entity.Property(e => e.TokenCriptografiaId)
            .UseSerialColumn()
            .HasColumnName("token_criptografia_id");

        entity.Property(e => e.AlteradoEm)
            .HasColumnType("timestamp")
            .HasColumnName("alterado_em");

        entity.Property(e => e.AlteradoPor)
            .HasColumnName("alterado_por");

        entity.Property(e => e.ClienteId)
            .HasColumnName("cliente_id");

        entity.Property(e => e.ExcluidoEm)
            .HasColumnType("timestamp")
            .HasColumnName("excluido_em");

        entity.Property(e => e.ExcluidoPor)
            .HasColumnName("excluido_por");

        entity.Property(e => e.IncluidoEm)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .HasColumnType("timestamp")
            .HasColumnName("incluido_em");

        entity.Property(e => e.IncluidoPor)
            .HasColumnName("incluido_por");

        entity.Property(e => e.Token)
            .HasDefaultValueSql("uuid_generate_v4()")
            .HasColumnName("token");
    }
}
