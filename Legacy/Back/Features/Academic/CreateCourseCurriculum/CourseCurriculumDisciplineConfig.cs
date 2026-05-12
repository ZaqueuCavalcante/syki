using Syki.Back.Features.Academic.CreateDiscipline;

namespace Syki.Back.Features.Academic.CreateCourseCurriculum;

public class CourseCurriculumDisciplineConfig : IEntityTypeConfiguration<CourseCurriculumDiscipline>
{
    public void Configure(EntityTypeBuilder<CourseCurriculumDiscipline> ccd)
    {
        ccd.ToTable("course_curriculums__disciplines");

        ccd.HasKey(x => new { x.CourseCurriculumId, x.DisciplineId });

        ccd.HasOne<CourseCurriculum>()
            .WithMany(g => g.Links)
            .HasForeignKey(x => x.CourseCurriculumId);

        ccd.HasOne<Discipline>()
            .WithMany()
            .HasForeignKey(x => x.DisciplineId);

        ccd.Property(x => x.PreRequisites)
            .HasColumnType("uuid[]")
            .IsRequired(false);
    }
}
