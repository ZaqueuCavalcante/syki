using Syki.Back.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Syki.Back.Database;

public class PeriodoDeMatriculaConfig : IEntityTypeConfiguration<PeriodoDeMatricula>
{
    public void Configure(EntityTypeBuilder<PeriodoDeMatricula> periodoDeMatricula)
    {
        periodoDeMatricula.ToTable("periodos_de_matricula");

        periodoDeMatricula.HasKey(p => new { p.Id, p.FaculdadeId });
        periodoDeMatricula.Property(p => p.Id).ValueGeneratedNever();
    }
}
