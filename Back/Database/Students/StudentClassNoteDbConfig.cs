using Estud.Back.Domain.Students;

namespace Estud.Back.Database.Students;

public class StudentClassNoteDbConfig : IEntityTypeConfiguration<StudentClassNote>
{
    public void Configure(EntityTypeBuilder<StudentClassNote> entity)
    {
        entity.ToTable("student_class_notes", DbSchemas.Estud);

        entity.HasKey(e => e.Id);

        entity.Property(e => e.Note).HasPrecision(4, 2);

        entity.HasOne<EstudStudent>()
            .WithMany()
            .HasPrincipalKey(x => x.Id)
            .HasForeignKey(e => e.StudentId);

        entity.HasIndex(e => new { e.ClassId, e.StudentId, e.Type })
            .IsUnique();
    }
}
