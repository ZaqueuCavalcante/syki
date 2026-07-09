using Estud.Back.Domain.CourseCurriculums;

namespace Estud.Back.Database.CourseCurriculums;

public class CourseCurriculumDisciplineDbConfig : IEntityTypeConfiguration<CourseCurriculumDiscipline>
{
    public void Configure(EntityTypeBuilder<CourseCurriculumDiscipline> entity)
    {
        entity.ToTable("course_curriculums_disciplines", DbSchemas.Estud);

        entity.HasKey(x => new { x.CourseCurriculumId, x.DisciplineId });
    }
}
