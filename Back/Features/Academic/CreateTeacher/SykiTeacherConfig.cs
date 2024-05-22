using Syki.Back.Features.Cross.CreateUser;

namespace Syki.Back.Features.Academic.CreateTeacher;

public class SykiTeacherConfig : IEntityTypeConfiguration<SykiTeacher>
{
    public void Configure(EntityTypeBuilder<SykiTeacher> teacher)
    {
        teacher.ToTable("teachers");

        teacher.HasKey(p => p.Id);
        teacher.Property(p => p.Id).ValueGeneratedNever();

        teacher.HasOne<SykiUser>()
            .WithOne()
            .HasPrincipalKey<SykiUser>(u => new { u.InstitutionId, u.Id })
            .HasForeignKey<SykiTeacher>(p => new { p.InstitutionId, p.Id });
    }
}
