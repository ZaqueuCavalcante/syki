using Syki.Back.CreateUser;

namespace Syki.Back.CreateInstitution;

public class FaculdadeConfig : IEntityTypeConfiguration<Faculdade>
{
    public void Configure(EntityTypeBuilder<Faculdade> faculdade)
    {
        faculdade.ToTable("faculdades");

        faculdade.HasKey(f => f.Id);
        faculdade.Property(f => f.Id).ValueGeneratedNever();

        faculdade.HasMany(f => f.AcademicPeriods)
            .WithOne()
            .HasForeignKey(p => p.InstitutionId);

        faculdade.HasMany(f => f.Campi)
            .WithOne()
            .HasForeignKey(c => c.InstitutionId);

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
            .HasForeignKey(a => a.InstitutionId);

        faculdade.HasMany<SykiUser>()
            .WithOne()
            .HasForeignKey(u => u.InstitutionId);

        faculdade.HasMany(f => f.Notifications)
            .WithOne()
            .HasForeignKey(n => n.FaculdadeId);
    }
}
