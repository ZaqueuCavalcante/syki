using Syki.Back.Domain.Courses;
using Syki.Back.Domain.Disciplines;

namespace Syki.Back.Database.Courses;

public class CourseDisciplineDbConfig : IEntityTypeConfiguration<CourseDiscipline>
{
    public void Configure(EntityTypeBuilder<CourseDiscipline> entity)
    {
        entity.ToTable("courses_disciplines", DbSchemas.Syki);

        entity.HasKey(x => new { x.CourseId, x.DisciplineId });

        entity.HasOne<Course>()
            .WithMany(c => c.Links)
            .HasPrincipalKey(c => c.Id)
            .HasForeignKey(e => e.CourseId);

        entity.HasOne<Discipline>()
            .WithMany(d => d.Links)
            .HasPrincipalKey(d => d.Id)
            .HasForeignKey(e => e.DisciplineId);
    }
}
