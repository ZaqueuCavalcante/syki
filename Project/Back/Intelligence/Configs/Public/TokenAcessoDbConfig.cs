using Exato.Back.Intelligence.Domain.Public;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Exato.Back.Intelligence.Configs.Public;

public class TokenAcessoDbConfig : IEntityTypeConfiguration<TokenAcesso>
{
    public const string TokenAcessoIdSeq = "token_acesso_token_acesso_id_seq";

    public void Configure(EntityTypeBuilder<TokenAcesso> entity)
    {
        entity.ToTable("token_acesso", "public", x =>
        {
            x.HasCheckConstraint("token_acesso_token_check", "char_length((token)::text) <= 32");
        });

        entity.HasKey(e => e.TokenAcessoId)
            .HasName("token_acesso_pk");

        entity.Property(e => e.TokenAcessoId)
            .ValueGeneratedOnAdd()
            .HasColumnName("token_acesso_id")
            .HasDefaultValueSql($"nextval('public.{TokenAcessoIdSeq}')");

        entity.Property(e => e.AcessoTotal)
            .HasColumnName("acesso_total");

        entity.Property(e => e.AlteradoEm)
            .HasColumnType("timestamp")
            .HasColumnName("alterado_em");

        entity.Property(e => e.AlteradoPor)
            .HasColumnName("alterado_por");

        entity.Property(e => e.BillingInstructions)
            .HasColumnType("jsonb")
            .HasColumnName("billing_instructions");

        entity.Property(e => e.ClienteId)
            .HasColumnName("cliente_id");

        entity.Property(e => e.CreditsLimitPerDay)
            .HasColumnName("credits_limit_per_day");

        entity.Property(e => e.CreditsLimitPerHour)
            .HasColumnName("credits_limit_per_hour");

        entity.Property(e => e.CreditsLimitPerMonth)
            .HasColumnName("credits_limit_per_month");

        entity.Property(e => e.CreditsLimitPerWeek)
            .HasColumnName("credits_limit_per_week");

        entity.Property(e => e.CurrencyLimitPerDay)
            .HasColumnName("currency_limit_per_day");

        entity.Property(e => e.CurrencyLimitPerHour)
            .HasColumnName("currency_limit_per_hour");

        entity.Property(e => e.CurrencyLimitPerMonth)
            .HasColumnName("currency_limit_per_month");

        entity.Property(e => e.CurrencyLimitPerWeek)
            .HasColumnName("currency_limit_per_week");

        entity.Property(e => e.Description)
            .HasColumnName("description");

        entity.Property(e => e.ExcluidoEm)
            .HasColumnType("timestamp")
            .HasColumnName("excluido_em");

        entity.Property(e => e.ExcluidoPor)
            .HasColumnName("excluido_por");

        entity.Property(e => e.IncluidoEm)
            .HasColumnType("timestamp")
            .HasColumnName("incluido_em");

        entity.Property(e => e.IncluidoPor)
            .HasColumnName("incluido_por");

        entity.Property(e => e.InsertTransaction)
            .HasColumnName("insert_transaction");

        entity.Property(e => e.IpsAutorizados)
            .HasColumnName("ips_autorizados");

        entity.Property(e => e.KeyId)
            .HasColumnName("key_id");

        entity.Property(e => e.KeyType)
            .HasColumnName("key_type");

        entity.Property(e => e.Name)
            .HasColumnName("name");

        entity.Property(e => e.NfGroup)
            .HasColumnName("nf_group");

        entity.Property(e => e.SecretHash)
            .HasColumnName("secret_hash");

        entity.Property(e => e.StoreTransactionInput)
            .HasColumnName("store_transaction_input");

        entity.Property(e => e.StoreTransactionReturn)
            .HasColumnName("store_transaction_return");

        entity.Property(e => e.Token)
            .HasColumnType("citext")
            .HasColumnName("token");

        entity.Property(e => e.TransLimitPerDay)
            .HasColumnName("trans_limit_per_day");

        entity.Property(e => e.TransLimitPerHour)
            .HasColumnName("trans_limit_per_hour");

        entity.Property(e => e.TransLimitPerMonth)
            .HasColumnName("trans_limit_per_month");

        entity.Property(e => e.TransLimitPerWeek)
            .HasColumnName("trans_limit_per_week");

        entity.Property(e => e.UsuarioId)
            .HasColumnName("usuario_id");

        entity.Property(e => e.ValidoAte)
            .HasColumnType("timestamp")
            .HasColumnName("valido_ate");
    }
}
