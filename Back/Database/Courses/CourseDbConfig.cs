using Syki.Back.Domain.Courses;

namespace Syki.Back.Database.Courses;

public class CourseDbConfig : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> entity)
    {
        entity.ToTable("courses", DbSchemas.Syki);

        entity.HasKey(e => e.Id);

        entity.HasMany(e => e.CourseCurriculums)
            .WithOne(cc => cc.Course)
            .HasForeignKey(cc => cc.CourseId);

        entity.HasMany(e => e.Disciplines)
            .WithMany()
            .UsingEntity<CourseDiscipline>();
    }
}
