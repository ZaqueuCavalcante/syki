using Syki.Back.Domain.Courses;

namespace Syki.Back.Database.Courses;

public class CourseCurriculumDbConfig : IEntityTypeConfiguration<CourseCurriculum>
{
    public void Configure(EntityTypeBuilder<CourseCurriculum> entity)
    {
        entity.ToTable("course_curriculums", DbSchemas.Syki);

        entity.HasKey(e => e.Id);

        entity.HasMany(e => e.Disciplines)
            .WithMany()
            .UsingEntity<CourseCurriculumDiscipline>();
    }
}
