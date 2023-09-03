using Syki.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Syki.Database;

public class CursoConfig : IEntityTypeConfiguration<Curso>
{
    public void Configure(EntityTypeBuilder<Curso> curso)
    {
        curso.ToTable("cursos");

        curso.HasKey(c => c.Id);
        curso.Property(c => c.Id).ValueGeneratedOnAdd();

        curso.Property(c => c.Tipo)
            .HasConversion(new EnumToStringConverter<TipoDeCurso>());

        curso.Property(c => c.Turno)
            .HasConversion(new EnumToStringConverter<Turno>());

        curso.HasMany(c => c.Grades)
            .WithOne()
            .HasForeignKey(g => g.CursoId);
    }
}
