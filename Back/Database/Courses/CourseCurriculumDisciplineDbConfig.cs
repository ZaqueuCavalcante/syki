using Syki.Back.Domain.Courses;

namespace Syki.Back.Database.Courses;

public class CourseCurriculumDisciplineDbConfig : IEntityTypeConfiguration<CourseCurriculumDiscipline>
{
    public void Configure(EntityTypeBuilder<CourseCurriculumDiscipline> entity)
    {
        entity.ToTable("course_curriculums_disciplines", DbSchemas.Syki);

        entity.HasKey(x => new { x.CourseCurriculumId, x.DisciplineId });
    }
}
