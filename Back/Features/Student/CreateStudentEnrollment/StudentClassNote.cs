using Syki.Back.Features.Teacher.AddClassActivityNote;

namespace Syki.Back.Features.Student.CreateStudentEnrollment;

/// <summary>
/// Representa uma Nota de um Aluno dentro de uma Turma
/// </summary>
public class StudentClassNote : Entity
{
    public Guid Id { get; set; }
    public Guid InstitutionId { get; set; }
    public Guid ClassId { get; set; }
    public Guid StudentId { get; set; }
    public StudentClassNoteType Type { get; set; }
    public decimal Note { get; set; }

    private StudentClassNote() {}

    public StudentClassNote(
        Guid institutionId,
        Guid classId,
        Guid studentId,
        StudentClassNoteType examType,
        decimal note
    ) {
        Id = Guid.NewGuid();
        InstitutionId = institutionId;
        ClassId = classId;
        StudentId = studentId;
        Type = examType;
        Note = note;
    }

    public OneOf<SykiSuccess, SykiError> AddNote(decimal note)
    {
        if (note < 0 || note > 10) return new InvalidStudentClassNote();

        Note = note;

        AddDomainEvent(new StudentClassNoteAddedDomainEvent(StudentId, ClassId));

        return new SykiSuccess();
    }

    public StudentClassNoteOut ToOut()
    {
        return new()
        {
            Id = Id,
            ClassId = ClassId,
            StudentId = StudentId,
            Type = Type,
            Note = Note,
        };
    }
}
