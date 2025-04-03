using Syki.Back.Features.Academic.CreateStudent;

namespace Syki.Back.Features.Teacher.AddClassActivityNote;

public class StudentClassNoteConfig : IEntityTypeConfiguration<StudentClassNote>
{
    public void Configure(EntityTypeBuilder<StudentClassNote> note)
    {
        note.ToTable("student_class_notes");

        note.HasKey(t => t.Id);
        note.Property(t => t.Id).ValueGeneratedNever();

        note.HasIndex(t => new { t.ClassId, t.StudentId, t.Type })
            .IsUnique();

        note.HasOne<SykiStudent>()
            .WithMany()
            .HasForeignKey(t => t.StudentId);

        note.Property(x => x.Note).HasPrecision(4, 2);
    }
}
