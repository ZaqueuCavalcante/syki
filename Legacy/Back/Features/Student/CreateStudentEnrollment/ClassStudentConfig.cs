using Estud.Back.Features.Academic.CreateClass;
using Estud.Back.Features.Academic.CreateStudent;

namespace Estud.Back.Features.Student.CreateStudentEnrollment;

public class ClassStudentConfig : IEntityTypeConfiguration<ClassStudent>
{
    public void Configure(EntityTypeBuilder<ClassStudent> classStudent)
    {
        classStudent.ToTable("classes__students");

        classStudent.HasKey(t => new { t.ClassId, t.EstudStudentId });

        classStudent.HasOne<Class>()
            .WithMany()
            .HasPrincipalKey(x => x.Id)
            .HasForeignKey(x => x.ClassId);

        classStudent.HasOne<EstudStudent>()
            .WithMany()
            .HasPrincipalKey(x => x.Id)
            .HasForeignKey(x => x.EstudStudentId);
    }
}
