using Estud.Back.Domain.Courses;
using Estud.Back.Domain.Disciplines;

namespace Estud.Back.Database.Courses;

public class CourseDbConfig : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> entity)
    {
        entity.ToTable("courses", DbSchemas.Estud);

        entity.HasKey(e => e.Id);

        entity.HasMany(e => e.CourseCurriculums)
            .WithOne(cc => cc.Course)
            .HasForeignKey(cc => cc.CourseId);

        entity.HasMany(e => e.Disciplines)
            .WithMany()
            .UsingEntity<CourseDiscipline>(
                right => right.HasOne<Discipline>().WithMany(d => d.Links).HasForeignKey(cd => cd.DisciplineId),
                left => left.HasOne<Course>().WithMany(c => c.Links).HasForeignKey(cd => cd.CourseId)
            );
    }
}
