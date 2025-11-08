using Exato.Back.Intelligence.Domain.Faturamento;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Exato.Back.Intelligence.Configs.Faturamento;

public class PlanoDbConfig : IEntityTypeConfiguration<Plano>
{
    public void Configure(EntityTypeBuilder<Plano> entity)
    {
        entity.ToTable("planos", "faturamento");

        entity.HasKey(e => e.Id)
            .HasName("planos_pkey");

        entity.Property(e => e.Id)
            .UseSerialColumn()
            .HasColumnName("id");

        entity.Property(e => e.DataAlteracao)
            .HasColumnType("timestamp")
            .HasColumnName("data_alteracao");

        entity.Property(e => e.DataExclusao)
            .HasColumnType("timestamp")
            .HasColumnName("data_exclusao");

        entity.Property(e => e.DataInclusao)
            .HasDefaultValueSql("now()")
            .HasColumnType("timestamp")
            .HasColumnName("data_inclusao");

        entity.Property(e => e.Descricao)
            .HasMaxLength(250)
            .HasColumnName("descricao");

        entity.Property(e => e.Nome)
            .HasMaxLength(100)
            .HasColumnName("nome");
    }
}
