namespace Syki.Back.Features.Teacher.AddClassActivityNote;

/// <summary>
/// Representa uma nota de um aluno em uma turma.
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
