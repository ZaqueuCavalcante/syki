using Exato.Back.Intelligence.Domain.Public;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Exato.Back.Intelligence.Configs.Public;

public class TokenAcessoConsultaTipoDbConfig : IEntityTypeConfiguration<TokenAcessoConsultaTipo>
{
    public const string TokenAcessoConsultaTipoIdSeq = "token_acesso_consulta_tipo_token_acesso_consulta_tipo_id_seq";

    public void Configure(EntityTypeBuilder<TokenAcessoConsultaTipo> entity)
    {
        entity.ToTable("token_acesso_consulta_tipo", "public");

        entity.HasKey(e => e.TokenAcessoConsultaTipoId)
            .HasName("token_acesso_consulta_tipo_pk");

        entity.Property(e => e.TokenAcessoConsultaTipoId)
            .ValueGeneratedOnAdd()
            .HasColumnName("token_acesso_consulta_tipo_id")
            .HasDefaultValueSql($"nextval('public.{TokenAcessoConsultaTipoIdSeq}')");

        entity.Property(e => e.ConsultaTipoId)
            .HasColumnName("consulta_tipo_id");

        entity.Property(e => e.IncluidoEm)
            .HasColumnType("timestamp")
            .HasColumnName("incluido_em");

        entity.Property(e => e.IncluidoPor)
            .HasColumnName("incluido_por");

        entity.Property(e => e.TokenAcessoId)
            .HasColumnName("token_acesso_id");
    }
}
