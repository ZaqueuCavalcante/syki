using Syki.Back.Features.Academic.CreateClass;
using Syki.Back.Features.Academic.CreateStudent;

namespace Syki.Back.Features.Teacher.CreateLessonAttendance;

public class ClassLessonAttendanceConfig : IEntityTypeConfiguration<ClassLessonAttendance>
{
    public void Configure(EntityTypeBuilder<ClassLessonAttendance> lessonAttendance)
    {
        lessonAttendance.ToTable("class_lesson_attendances");

        lessonAttendance.HasKey(t => t.Id);
        lessonAttendance.Property(t => t.Id).ValueGeneratedNever();

        lessonAttendance.HasIndex(t => new { t.LessonId, t.StudentId })
            .IsUnique();

        lessonAttendance.HasOne<SykiStudent>()
            .WithMany()
            .HasForeignKey(t => t.StudentId);

        lessonAttendance.HasOne<Class>()
            .WithMany()
            .HasForeignKey(t => t.ClassId);
    }
}
