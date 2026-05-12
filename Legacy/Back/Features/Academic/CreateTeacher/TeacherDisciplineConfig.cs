using Syki.Back.Features.Academic.CreateDiscipline;

namespace Syki.Back.Features.Academic.CreateTeacher;

public class TeacherDisciplineConfig : IEntityTypeConfiguration<TeacherDiscipline>
{
    public void Configure(EntityTypeBuilder<TeacherDiscipline> cd)
    {
        cd.ToTable("teachers__disciplines");

        cd.HasKey(x => new { x.SykiTeacherId, x.DisciplineId });

        cd.HasOne<SykiTeacher>()
            .WithMany()
            .HasPrincipalKey(x => x.Id)
            .HasForeignKey(x => x.SykiTeacherId);

        cd.HasOne<Discipline>()
            .WithMany()
            .HasPrincipalKey(x => x.Id)
            .HasForeignKey(x => x.DisciplineId);
    }
}
