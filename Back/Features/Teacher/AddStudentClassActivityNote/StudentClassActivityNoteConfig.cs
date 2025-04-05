using Syki.Back.Features.Academic.CreateStudent;
using Syki.Back.Features.Teacher.CreateClassActivity;

namespace Syki.Back.Features.Teacher.AddStudentClassActivityNote;

public class StudentClassActivityNoteConfig : IEntityTypeConfiguration<StudentClassActivityNote>
{
    public void Configure(EntityTypeBuilder<StudentClassActivityNote> note)
    {
        note.ToTable("student_class_activity_notes");

        note.HasKey(t => t.Id);
        note.Property(t => t.Id).ValueGeneratedNever();

        note.HasIndex(t => new { t.StudentId, t.ClassActivityId })
            .IsUnique();

        note.HasOne<SykiStudent>()
            .WithMany()
            .HasForeignKey(t => t.StudentId);

        note.HasOne<ClassActivity>()
            .WithMany()
            .HasForeignKey(t => t.ClassActivityId);

        note.Property(x => x.Value).HasPrecision(4, 2);
    }
}
