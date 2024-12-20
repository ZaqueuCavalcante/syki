using Syki.Back.Features.Teacher.AddExamGradeNote;

namespace Syki.Back.Features.Student.CreateStudentEnrollment;

/// <summary>
/// Representa uma Prova.
/// </summary>
public class ExamGrade : Entity
{
    public Guid Id { get; set; }
    public Guid ClassId { get; set; }
    public Guid StudentId { get; set; }
    public ExamType ExamType { get; set; }
    public decimal Note { get; set; }

    private ExamGrade() {}

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

    public OneOf<SykiSuccess, SykiError> AddNote(decimal note)
    {
        if (note < 0 || note > 10) return new InvalidExamGradeNote();

        Note = note;

        AddDomainEvent(new ExamGradeNoteAddedDomainEvent(StudentId, ClassId));

        return new SykiSuccess();
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
