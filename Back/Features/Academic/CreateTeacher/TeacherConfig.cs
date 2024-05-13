using Syki.Back.Features.Cross.CreateUser;

namespace Syki.Back.Features.Academic.CreateTeacher;

public class ProfessorConfig : IEntityTypeConfiguration<Teacher>
{
    public void Configure(EntityTypeBuilder<Teacher> professor)
    {
        professor.ToTable("professores");

        professor.HasKey(p => p.Id);
        professor.Property(p => p.Id).ValueGeneratedNever();

        professor.HasOne<SykiUser>()
            .WithOne()
            .HasPrincipalKey<SykiUser>(u => new { u.InstitutionId, u.Id })
            .HasForeignKey<Teacher>(p => new { p.InstitutionId, p.Id });
    }
}
