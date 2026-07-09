using Estud.Back.Domain.Courses;

namespace Estud.Back.Database.Courses;

public class CourseDisciplineDbConfig : IEntityTypeConfiguration<CourseDiscipline>
{
    public void Configure(EntityTypeBuilder<CourseDiscipline> entity)
    {
        entity.ToTable("courses_disciplines", DbSchemas.Estud);

        entity.HasKey(x => new { x.CourseId, x.DisciplineId });
    }
}
