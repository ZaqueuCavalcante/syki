using Syki.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Syki.Database;

public class FaculdadeConfig : IEntityTypeConfiguration<Faculdade>
{
    public void Configure(EntityTypeBuilder<Faculdade> faculdade)
    {
        faculdade.ToTable("faculdades");

        faculdade.HasKey(f => f.Id);
        faculdade.Property(f => f.Id).ValueGeneratedOnAdd();

        faculdade.HasMany(f => f.Campi)
            .WithOne()
            .HasForeignKey(c => c.FaculdadeId);

        faculdade.HasMany(f => f.Cursos)
            .WithOne()
            .HasForeignKey(c => c.FaculdadeId);

        faculdade.HasMany(f => f.Ofertas)
            .WithOne()
            .HasForeignKey(co => co.FaculdadeId);

        faculdade.HasMany(f => f.Grades)
            .WithOne()
            .HasForeignKey(g => g.FaculdadeId);

        faculdade.HasMany(f => f.Disciplinas)
            .WithOne()
            .HasForeignKey(d => d.FaculdadeId);

        faculdade.HasMany(f => f.Professores)
            .WithOne()
            .HasForeignKey(p => p.FaculdadeId);

        faculdade.HasMany(f => f.Alunos)
            .WithOne()
            .HasForeignKey(a => a.FaculdadeId);
    }
}
