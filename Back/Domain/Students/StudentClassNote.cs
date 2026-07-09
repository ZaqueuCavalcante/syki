namespace Estud.Back.Domain.Students;

/// <summary>
/// Representa uma nota de um aluno em uma turma.
/// </summary>
public class StudentClassNote
{
    public int Id { get; set; }
    public int ClassId { get; set; }
    public int StudentId { get; set; }
    public ClassNoteType Type { get; set; }
    public decimal Note { get; set; }

    private StudentClassNote() {}

    public StudentClassNote(
        int classId,
        int studentId,
        ClassNoteType examType,
        decimal note
    ) {
        ClassId = classId;
        StudentId = studentId;
        Type = examType;
        Note = note;
    }
}
