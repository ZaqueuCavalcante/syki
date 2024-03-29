using Syki.Back.CreateProfessor;
using Syki.Back.CreateUser;

namespace Syki.Back.Database;

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
            .HasForeignKey<Professor>(p => new { p.FaculdadeId, p.Id });
    }
}
