using Syki.Back.Features.Academic.CreateDiscipline;

namespace Syki.Back.Features.Academic.CreateCourse;

public class CourseConfig : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> course)
    {
        course.ToTable("courses");

        course.HasKey(c => c.Id);
        course.Property(c => c.Id).ValueGeneratedNever();

        course.HasMany(c => c.CourseCurriculums)
            .WithOne(g => g.Course)
            .HasForeignKey(g => g.CourseId);

        course.HasMany(c => c.Disciplines)
            .WithMany()
            .UsingEntity<CourseDiscipline>();
    }
}
