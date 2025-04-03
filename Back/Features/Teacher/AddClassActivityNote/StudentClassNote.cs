namespace Syki.Back.Features.Teacher.AddClassActivityNote;

/// <summary>
/// Representa uma nota de um aluno em uma atividade
/// </summary>
public class StudentClassNote : Entity
{
    public Guid Id { get; set; }
    public Guid ClassId { get; set; }
    public Guid StudentId { get; set; }
    public ClassNoteType Type { get; set; }
    public decimal Note { get; set; }

    private StudentClassNote() {}

    public StudentClassNote(
        Guid classId,
        Guid studentId,
        ClassNoteType examType,
        decimal note
    ) {
        Id = Guid.NewGuid();
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
