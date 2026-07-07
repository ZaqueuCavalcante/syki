using Syki.Back.Domain.Classes;
using Syki.Back.Domain.Students;

namespace Syki.Back.Database.Classes;

public class ClassLessonAttendanceDbConfig : IEntityTypeConfiguration<ClassLessonAttendance>
{
    public void Configure(EntityTypeBuilder<ClassLessonAttendance> entity)
    {
        entity.ToTable("class_lesson_attendances", DbSchemas.Syki);

        entity.HasKey(t => t.Id);

        entity.HasIndex(t => new { t.LessonId, t.StudentId })
            .IsUnique();

        entity.HasOne<SykiStudent>()
            .WithMany()
            .HasForeignKey(t => t.StudentId);

        entity.HasOne<Class>()
            .WithMany()
            .HasForeignKey(t => t.ClassId);
    }
}
