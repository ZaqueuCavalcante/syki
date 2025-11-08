using Exato.Back.Intelligence.Domain.Public;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Exato.Back.Intelligence.Configs.Public;

public class CaptchaMecanismoDbConfig : IEntityTypeConfiguration<CaptchaMecanismo>
{
    public void Configure(EntityTypeBuilder<CaptchaMecanismo> entity)
    {
        entity.ToTable("captcha_mecanismo", "public", x =>
        {
            x.HasCheckConstraint("captcha_mecanismo_nome_check", "char_length((nome)::text) <= 50");
        });

        entity.HasKey(e => e.CaptchaMecanismoId)
            .HasName("captcha_mecanismo_pk");

        entity.Property(e => e.CaptchaMecanismoId)
            .ValueGeneratedNever()
            .HasColumnName("captcha_mecanismo_id");

        entity.Property(e => e.AlteradoEm)
            .HasColumnType("timestamp")
            .HasColumnName("alterado_em");

        entity.Property(e => e.AlteradoPor)
            .HasColumnName("alterado_por");

        entity.Property(e => e.Ativo)
            .HasColumnName("ativo");

        entity.Property(e => e.Host)
            .HasMaxLength(50)
            .HasColumnName("host");

        entity.Property(e => e.IncorretoQuantidade)
            .HasDefaultValue(0)
            .HasColumnName("incorreto_quantidade");

        entity.Property(e => e.NaoResolvidoQuantidade)
            .HasDefaultValue(0)
            .HasColumnName("nao_resolvido_quantidade");

        entity.Property(e => e.NaoResolvidoUltimoData)
            .HasColumnType("timestamp")
            .HasColumnName("nao_resolvido_ultimo_data");

        entity.Property(e => e.Nome)
            .HasColumnType("citext")
            .HasColumnName("nome");

        entity.Property(e => e.OmieSourceId)
            .HasColumnName("omie_source_id");

        entity.Property(e => e.Password)
            .HasMaxLength(50)
            .HasColumnName("password");

        entity.Property(e => e.Port)
            .HasColumnName("port");

        entity.Property(e => e.ResolvidoQuantidade)
            .HasDefaultValue(0)
            .HasColumnName("resolvido_quantidade");

        entity.Property(e => e.ResolvidoUltimoData)
            .HasColumnType("timestamp")
            .HasColumnName("resolvido_ultimo_data");

        entity.Property(e => e.SucessoQuantidade)
            .HasDefaultValue(0)
            .HasColumnName("sucesso_quantidade");

        entity.Property(e => e.TempoMedioSolucaoCorretaMs)
            .HasColumnName("tempo_medio_solucao_correta_ms");

        entity.Property(e => e.TimeoutMs)
            .HasColumnName("timeout_ms");

        entity.Property(e => e.Username)
            .HasMaxLength(50)
            .HasColumnName("username");

        entity.Property(e => e.UtilizacaoQuantidade)
            .HasDefaultValue(0)
            .HasColumnName("utilizacao_quantidade");

        entity.Property(e => e.UtilizacaoUltimoData)
            .HasColumnType("timestamp")
            .HasColumnName("utilizacao_ultimo_data");

        entity.HasIndex(e => e.Nome, "captcha_mecanismo_ak_nome")
            .IsUnique();
    }
}
