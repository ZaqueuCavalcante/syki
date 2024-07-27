namespace Syki.Back.Features.Student.CreateStudentEnrollment;

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
        ExamType examType,
        decimal note
    ) {
        Id = Guid.NewGuid();
        ClassId = classId;
        StudentId = studentId;
        ExamType = examType;
        Note = note;
    }

    public void AddNote(decimal note)
    {
        Note = note;
    }

    public ExamGradeOut ToOut()
    {
        return new()
        {
            Id = Id,
            ClassId = ClassId,
            StudentId = StudentId,
            ExamType = ExamType,
            Note = Note,
        };
    }
}
