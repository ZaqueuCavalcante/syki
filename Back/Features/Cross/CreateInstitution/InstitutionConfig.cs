namespace Syki.Back.CreateInstitution;

public class InstitutionConfig : IEntityTypeConfiguration<Institution>
{
    public void Configure(EntityTypeBuilder<Institution> institution)
    {
        institution.ToTable("institutions");

        institution.HasKey(f => f.Id);
        institution.Property(f => f.Id).ValueGeneratedNever();

        institution.HasMany(f => f.AcademicPeriods)
            .WithOne()
            .HasForeignKey(p => p.InstitutionId);

        institution.HasMany(f => f.Campi)
            .WithOne()
            .HasForeignKey(c => c.InstitutionId);

        institution.HasMany(f => f.Cursos)
            .WithOne()
            .HasForeignKey(c => c.InstitutionId);

        institution.HasMany(f => f.Ofertas)
            .WithOne()
            .HasForeignKey(co => co.InstitutionId);

        institution.HasMany(f => f.Grades)
            .WithOne()
            .HasForeignKey(g => g.InstitutionId);

        institution.HasMany(f => f.Disciplinas)
            .WithOne()
            .HasForeignKey(d => d.InstitutionId);

        institution.HasMany(f => f.Professores)
            .WithOne()
            .HasForeignKey(p => p.InstitutionId);

        institution.HasMany(f => f.Alunos)
            .WithOne()
            .HasForeignKey(a => a.InstitutionId);

        institution.HasMany<SykiUser>()
            .WithOne()
            .HasForeignKey(u => u.InstitutionId);

        institution.HasMany(f => f.Notifications)
            .WithOne()
            .HasForeignKey(n => n.InstitutionId);
    }
}
