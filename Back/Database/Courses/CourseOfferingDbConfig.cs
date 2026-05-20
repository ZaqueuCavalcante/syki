using Syki.Back.Domain.Courses;

namespace Syki.Back.Database.Courses;

public class CourseOfferingDbConfig : IEntityTypeConfiguration<CourseOffering>
{
    public void Configure(EntityTypeBuilder<CourseOffering> entity)
    {
        entity.ToTable("course_offerings", DbSchemas.Syki);

        entity.HasKey(e => e.Id);

        entity.HasOne(e => e.Campus)
            .WithMany()
            .HasForeignKey(e => e.CampusId);

        entity.HasOne(e => e.Course)
            .WithMany()
            .HasForeignKey(e => e.CourseId);

        entity.HasOne(e => e.CourseCurriculum)
            .WithMany()
            .HasForeignKey(e => e.CourseCurriculumId);

        entity.HasOne(e => e.AcademicPeriod)
            .WithMany()
            .HasForeignKey(e => e.AcademicPeriodId);
    }
}
