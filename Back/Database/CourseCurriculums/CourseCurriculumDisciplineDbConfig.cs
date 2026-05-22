using Syki.Back.Domain.CourseCurriculums;

namespace Syki.Back.Database.CourseCurriculums;

public class CourseCurriculumDisciplineDbConfig : IEntityTypeConfiguration<CourseCurriculumDiscipline>
{
    public void Configure(EntityTypeBuilder<CourseCurriculumDiscipline> entity)
    {
        entity.ToTable("course_curriculums_disciplines", DbSchemas.Syki);

        entity.HasKey(x => new { x.CourseCurriculumId, x.DisciplineId });
    }
}
