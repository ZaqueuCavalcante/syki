using Syki.Back.Features.Academic.CreateCourse;

namespace Syki.Back.Features.Academic.CreateDiscipline;

public class CourseDisciplineConfig : IEntityTypeConfiguration<CourseDiscipline>
{
    public void Configure(EntityTypeBuilder<CourseDiscipline> cd)
    {
        cd.ToTable("courses__disciplines");

        cd.HasKey(x => new { x.CourseId, x.DisciplineId });

        cd.HasOne<Course>()
            .WithMany(c => c.Links)
            .HasPrincipalKey(x => x.Id)
            .HasForeignKey(x => x.CourseId);

        cd.HasOne<Discipline>()
            .WithMany(d => d.Links)
            .HasPrincipalKey(x => x.Id)
            .HasForeignKey(x => x.DisciplineId);
    }
}
