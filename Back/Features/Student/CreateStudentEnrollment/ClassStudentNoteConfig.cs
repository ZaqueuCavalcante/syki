using Syki.Back.Features.Academic.CreateStudent;
using Syki.Back.Features.Cross.CreateInstitution;

namespace Syki.Back.Features.Student.CreateStudentEnrollment;

public class ClassStudentNoteConfig : IEntityTypeConfiguration<ClassStudentNote>
{
    public void Configure(EntityTypeBuilder<ClassStudentNote> note)
    {
        note.ToTable("class_student_notes");

        note.HasKey(t => t.Id);
        note.Property(t => t.Id).ValueGeneratedNever();

        note.HasOne<Institution>()
            .WithMany()
            .HasPrincipalKey(u => u.Id)
            .HasForeignKey(rp => rp.InstitutionId);

        note.HasIndex(t => new { t.ClassId, t.StudentId, t.ClassStudentNoteType })
            .IsUnique();

        note.HasOne<SykiStudent>()
            .WithMany()
            .HasForeignKey(t => t.StudentId);

        note.Property(x => x.Note).HasPrecision(4, 2);
    }
}
