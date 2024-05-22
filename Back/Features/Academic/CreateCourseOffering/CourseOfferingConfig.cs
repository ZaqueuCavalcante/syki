using Syki.Back.Features.Academic.CreateStudent;
using Syki.Back.Features.Academic.CreateAcademicPeriod;

namespace Syki.Back.Features.Academic.CreateCourseOffering;

public class OfertaConfig : IEntityTypeConfiguration<CourseOffering>
{
    public void Configure(EntityTypeBuilder<CourseOffering> courseOffering)
    {
        courseOffering.ToTable("course_offerings");

        courseOffering.HasKey(o => o.Id);
        courseOffering.Property(o => o.Id).ValueGeneratedNever();

        courseOffering.HasOne(o => o.Course)
            .WithMany()
            .HasForeignKey(o => o.CourseId);

        courseOffering.HasOne(o => o.CourseCurriculum)
            .WithMany()
            .HasForeignKey(o => o.CourseCurriculumId);

        courseOffering.HasOne<AcademicPeriod>()
            .WithMany()
            .HasForeignKey(o => new { o.Period, o.InstitutionId });

        courseOffering.HasMany<SykiStudent>()
            .WithOne(a => a.CourseOffering)
            .HasForeignKey(a => a.CourseOfferingId);
    }
}
