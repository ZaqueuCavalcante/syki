using Syki.Back.Features.Academic.CreateClass;
using Syki.Back.Features.Academic.CreateStudent;

namespace Syki.Back.Features.Student.CreateStudentEnrollment;

public class ClassStudentConfig : IEntityTypeConfiguration<ClassStudent>
{
    public void Configure(EntityTypeBuilder<ClassStudent> classStudent)
    {
        classStudent.ToTable("classes__students");

        classStudent.HasKey(t => new { t.ClassId, t.StudentId });

        classStudent.HasOne<Class>()
            .WithMany()
            .HasPrincipalKey(x => x.Id)
            .HasForeignKey(x => x.ClassId);

        classStudent.HasOne<SykiStudent>()
            .WithMany()
            .HasPrincipalKey(x => x.Id)
            .HasForeignKey(x => x.StudentId);
    }
}
