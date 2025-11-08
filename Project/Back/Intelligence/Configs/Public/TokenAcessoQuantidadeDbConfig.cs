using Exato.Back.Intelligence.Domain.Public;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Exato.Back.Intelligence.Configs.Public;

public class TokenAcessoQuantidadeDbConfig : IEntityTypeConfiguration<TokenAcessoQuantidade>
{
    public void Configure(EntityTypeBuilder<TokenAcessoQuantidade> entity)
    {
        entity.ToTable("token_acesso_quantidades", "public");

        entity.HasKey(e => new { e.TokenAcessoId, e.Day, e.Hour })
            .HasName("token_acesso_quantidades_pk");

        entity.Property(e => e.TokenAcessoId)
            .HasColumnName("token_acesso_id");

        entity.Property(e => e.Day)
            .HasColumnName("day");

        entity.Property(e => e.Hour)
            .HasColumnType("time")
            .HasColumnName("hour");

        entity.Property(e => e.CreditsTotal)
            .HasColumnName("credits_total");

        entity.Property(e => e.CurrencyTotal)
            .HasColumnName("currency_total");

        entity.Property(e => e.TransTotal)
            .HasColumnName("trans_total");
    }
}
