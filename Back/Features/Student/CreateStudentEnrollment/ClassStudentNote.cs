using Syki.Back.Features.Teacher.AddExamGradeNote;

namespace Syki.Back.Features.Student.CreateStudentEnrollment;

/// <summary>
/// Representa uma Nota de um Aluno dentro de uma Turma
/// </summary>
public class ClassStudentNote : Entity
{
    public Guid Id { get; set; }
    public Guid InstitutionId { get; set; }
    public Guid ClassId { get; set; }
    public Guid StudentId { get; set; }
    public ClassStudentNoteType ClassStudentNoteType { get; set; }
    public decimal Note { get; set; }

    private ClassStudentNote() {}

    public ClassStudentNote(
        Guid institutionId,
        Guid classId,
        Guid studentId,
        ClassStudentNoteType examType,
        decimal note
    ) {
        Id = Guid.NewGuid();
        InstitutionId = institutionId;
        ClassId = classId;
        StudentId = studentId;
        ClassStudentNoteType = examType;
        Note = note;
    }

    public OneOf<SykiSuccess, SykiError> AddNote(decimal note)
    {
        if (note < 0 || note > 10) return new InvalidExamGradeNote();

        Note = note;

        AddDomainEvent(new ExamGradeNoteAddedDomainEvent(StudentId, ClassId));

        return new SykiSuccess();
    }

    public ClassStudentNoteOut ToOut()
    {
        return new()
        {
            Id = Id,
            ClassId = ClassId,
            StudentId = StudentId,
            ClassStudentNoteType = ClassStudentNoteType,
            Note = Note,
        };
    }
}
