using Syki.Dtos;
using Syki.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Syki.Database;

public class CursoOfertadoConfig : IEntityTypeConfiguration<CursoOfertado>
{
    public void Configure(EntityTypeBuilder<CursoOfertado> cursoOfertado)
    {
        cursoOfertado.ToTable("cursos_ofertados");

        cursoOfertado.HasKey(co => co.Id);
        cursoOfertado.Property(co => co.Id).ValueGeneratedOnAdd();

        cursoOfertado.HasOne<Curso>()
            .WithMany()
            .HasForeignKey(co => co.CursoId);

        cursoOfertado.HasOne<Grade>()
            .WithMany()
            .HasForeignKey(co => co.GradeId);

        cursoOfertado.HasOne<Periodo>()
            .WithMany()
            .HasForeignKey(co => new { co.PeriodoId, co.FaculdadeId });

        cursoOfertado.Property(c => c.Turno)
            .HasConversion(new EnumToStringConverter<Turno>());
    }
}
