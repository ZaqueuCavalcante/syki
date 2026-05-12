namespace Syki.Back.Features.Academic.CreateCourseCurriculum;

public class CourseCurriculumConfig : IEntityTypeConfiguration<CourseCurriculum>
{
    public void Configure(EntityTypeBuilder<CourseCurriculum> courseCurriculum)
    {
        courseCurriculum.ToTable("course_curriculums");

        courseCurriculum.HasKey(g => g.Id);
        courseCurriculum.Property(g => g.Id).ValueGeneratedNever();

        courseCurriculum.HasMany(g => g.Disciplines)
            .WithMany()
            .UsingEntity<CourseCurriculumDiscipline>();
    }
}
