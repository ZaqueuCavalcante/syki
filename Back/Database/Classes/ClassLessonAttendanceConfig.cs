using Syki.Back.Domain.Students;
using Syki.Back.Commands.Domain.Classes;

namespace Syki.Back.Database.Classes;

public class ClassLessonAttendanceConfig : IEntityTypeConfiguration<ClassLessonAttendance>
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
