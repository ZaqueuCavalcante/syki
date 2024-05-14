using Syki.Back.Features.Academic.CreateDiscipline;

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
            .UsingEntity<CourseCurriculumDiscipline>(gd =>
                {
                    gd.ToTable("course_curriculums__disciplines");
                    gd.HasOne<CourseCurriculum>().WithMany(g => g.Links).HasForeignKey(x => x.CourseCurriculumId);
                    gd.HasOne<Discipline>().WithMany().HasForeignKey(x => x.DisciplineId);
                }
            );
    }
}
