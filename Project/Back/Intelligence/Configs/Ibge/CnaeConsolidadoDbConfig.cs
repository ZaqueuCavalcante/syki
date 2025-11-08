using Exato.Back.Intelligence.Domain.Ibge;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Exato.Back.Intelligence.Configs.Ibge;

public class CnaeConsolidadoDbConfig : IEntityTypeConfiguration<CnaeConsolidado>
{
    public void Configure(EntityTypeBuilder<CnaeConsolidado> entity)
    {
        entity.ToTable("cnae_consolidado", "ibge");

        entity.HasKey(e => e.Id);

        entity.Property(e => e.Id)
            .UseSerialColumn()
            .HasColumnName("id");

        entity.Property(e => e.Classe)
            .UseCollation("C")
            .HasColumnName("classe");

        entity.Property(e => e.ClasseNum)
            .HasColumnName("classe_num");

        entity.Property(e => e.ControleId)
            .HasColumnName("controle_id");

        entity.Property(e => e.Denominacao)
            .UseCollation("C")
            .HasColumnName("denominacao");

        entity.Property(e => e.Divisao)
            .UseCollation("C")
            .HasColumnName("divisao");

        entity.Property(e => e.DivisaoNum)
            .HasColumnName("divisao_num");

        entity.Property(e => e.Grupo)
            .UseCollation("C")
            .HasColumnName("grupo");

        entity.Property(e => e.GrupoNum)
            .HasColumnName("grupo_num");

        entity.Property(e => e.Secao)
            .UseCollation("C")
            .HasColumnName("secao");

        entity.Property(e => e.SegmentoQuod)
            .HasColumnName("segmento_quod");

        entity.Property(e => e.Subclasse)
            .UseCollation("C")
            .HasColumnName("subclasse");

        entity.Property(e => e.SubclasseNum)
            .HasColumnName("subclasse_num");

        entity.Property(e => e.Tipo)
            .UseCollation("C")
            .HasColumnName("tipo");

        entity.Property(e => e.Versao)
            .HasPrecision(2, 1)
            .HasColumnName("versao");
    }
}
