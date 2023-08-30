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

        curso.HasOne<Grade>()
            .WithOne()
            .HasPrincipalKey<Grade>(g => g.Id)
            .HasForeignKey<Curso>(c => c.GradeId);

        curso.Property(c => c.Turno)
            .HasConversion(new EnumToStringConverter<Turno>());
    }
}
