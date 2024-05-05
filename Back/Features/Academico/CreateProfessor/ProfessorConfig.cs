namespace Syki.Back.CreateProfessor;

public class ProfessorConfig : IEntityTypeConfiguration<Professor>
{
    public void Configure(EntityTypeBuilder<Professor> professor)
    {
        professor.ToTable("professores");

        professor.HasKey(p => p.Id);
        professor.Property(p => p.Id).ValueGeneratedNever();

        professor.HasOne<SykiUser>()
            .WithOne()
            .HasPrincipalKey<SykiUser>(u => new { u.InstitutionId, u.Id })
            .HasForeignKey<Professor>(p => new { p.InstitutionId, p.Id });
    }
}
