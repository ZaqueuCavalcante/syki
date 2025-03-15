namespace Syki.Back.Features.Teacher.CreateClassActivity;

public class StudentClassActivityNoteConfig : IEntityTypeConfiguration<StudentClassActivityNote>
{
    public void Configure(EntityTypeBuilder<StudentClassActivityNote> note)
    {
        note.ToTable("student_class_activity_notes");

        note.HasKey(t => t.Id);
        note.Property(t => t.Id).ValueGeneratedNever();

        note.Property(x => x.Value).HasPrecision(4, 2);
    }
}
