using Syki.Back.Features.Academic.CreateStudent;
using Syki.Back.Features.Cross.CreateInstitution;

namespace Syki.Back.Features.Student.CreateStudentEnrollment;

public class ExamGradeConfig : IEntityTypeConfiguration<ExamGrade>
{
    public void Configure(EntityTypeBuilder<ExamGrade> examGrade)
    {
        examGrade.ToTable("exam_grades");

        examGrade.HasKey(t => t.Id);
        examGrade.Property(t => t.Id).ValueGeneratedNever();

        examGrade.HasOne<Institution>()
            .WithMany()
            .HasPrincipalKey(u => u.Id)
            .HasForeignKey(rp => rp.InstitutionId);

        examGrade.HasIndex(t => new { t.ClassId, t.StudentId, t.ExamType })
            .IsUnique();

        examGrade.HasOne<SykiStudent>()
            .WithMany()
            .HasForeignKey(t => t.StudentId);

        examGrade.Property(x => x.Note).HasPrecision(4, 2);
    }
}
