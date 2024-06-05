namespace Syki.Back.Features.Academic.StartClass;

public class ExamGrade
{
    public Guid Id { get; set; }
    public Guid ClassId { get; set; }
    public Guid StudentId { get; set; }
    public ExamType ExamType { get; set; }
    public decimal Note { get; set; }

    public ExamGrade(
        Guid classId,
        Guid studentId,
        ExamType examType
    ) {
        Id = Guid.NewGuid();
        ClassId = classId;
        StudentId = studentId;
        ExamType = examType;
        Note = 0.00M;
    }
}
