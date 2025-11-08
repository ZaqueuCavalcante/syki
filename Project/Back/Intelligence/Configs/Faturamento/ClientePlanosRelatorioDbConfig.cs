using Exato.Back.Intelligence.Domain.Public;
using Exato.Back.Intelligence.Domain.Faturamento;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Exato.Back.Intelligence.Configs.Faturamento;

public class ClientePlanosRelatorioDbConfig : IEntityTypeConfiguration<ClientePlanosRelatorio>
{
    public void Configure(EntityTypeBuilder<ClientePlanosRelatorio> entity)
    {
        entity.ToTable("cliente_planos_relatorios", "faturamento");

        entity.HasKey(e => e.Id)
            .HasName("cliente_planos_relatorios_pkey");

        entity.Property(e => e.Id)
            .UseSerialColumn()
            .HasColumnName("id");

        entity.Property(e => e.Ativo)
            .HasDefaultValue(true)
            .HasColumnName("ativo");

        entity.Property(e => e.ClienteId)
            .HasColumnName("cliente_id");

        entity.Property(e => e.DataAtribuicao)
            .HasDefaultValueSql("now()")
            .HasColumnType("timestamp")
            .HasColumnName("data_atribuicao");

        entity.Property(e => e.DataDesativacao)
            .HasColumnType("timestamp")
            .HasColumnName("data_desativacao");

        entity.Property(e => e.PlanoId)
            .HasColumnName("plano_id");

        entity.HasOne<Cliente>()
            .WithMany()
            .HasPrincipalKey(x => x.ClienteId)
            .HasForeignKey(x => x.ClienteId);

        entity.HasOne<Plano>()
            .WithMany()
            .HasPrincipalKey(x => x.Id)
            .HasForeignKey(x => x.PlanoId);

        entity.HasIndex(e => new { e.ClienteId, e.PlanoId }, "uq_cliente_plano")
            .IsUnique();
    }
}
