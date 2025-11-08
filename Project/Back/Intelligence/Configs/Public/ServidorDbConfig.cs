using Exato.Back.Intelligence.Domain.Public;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Exato.Back.Intelligence.Configs.Public;

public class ServidorDbConfig : IEntityTypeConfiguration<Servidor>
{
    public void Configure(EntityTypeBuilder<Servidor> entity)
    {
        entity.ToTable("servidor", "public");

        entity.HasKey(e => e.ServidorId)
            .HasName("servidor_pk");

        entity.Property(e => e.ServidorId)
            .ValueGeneratedNever()
            .HasColumnName("servidor_id");

        entity.Property(e => e.IpInternoEntrada)
            .HasMaxLength(15)
            .HasColumnName("ip_interno_entrada");

        entity.Property(e => e.NomeMaquina)
            .HasColumnType("citext")
            .HasColumnName("nome_maquina");

        entity.Property(e => e.ProcessarLotes)
            .HasColumnName("processar_lotes");
    }
}
