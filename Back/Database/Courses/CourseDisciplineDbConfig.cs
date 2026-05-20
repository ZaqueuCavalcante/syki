using Syki.Back.Domain.Courses;

namespace Syki.Back.Database.Courses;

public class CourseDisciplineDbConfig : IEntityTypeConfiguration<CourseDiscipline>
{
    public void Configure(EntityTypeBuilder<CourseDiscipline> entity)
    {
        entity.ToTable("courses_disciplines", DbSchemas.Syki);

        entity.HasKey(x => new { x.CourseId, x.DisciplineId });
    }
}
