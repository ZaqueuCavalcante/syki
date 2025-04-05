namespace Syki.Back.Features.Teacher.AddStudentClassActivityNote;

/// <summary>
/// Representa uma nota de um aluno em uma atividade
/// </summary>
public class StudentClassActivityNote : Entity
{
    public Guid Id { get; set; }
    public Guid StudentId { get; set; }
    public Guid ClassActivityId { get; set; }
    public decimal Value { get; set; }

    private StudentClassActivityNote() {}

    private StudentClassActivityNote(
        Guid studentId,
        Guid classActivityId,
        decimal value
    ) {
        Id = Guid.NewGuid();
        StudentId = studentId;
        ClassActivityId = classActivityId;
        Value = value;
    }

    public static OneOf<StudentClassActivityNote, SykiError> New(
        Guid studentId,
        Guid classActivityId,
        decimal value
    ) {
        if (value < 0 || value > 10) return new InvalidStudentClassActivityNote();

        return new StudentClassActivityNote(studentId, classActivityId, value);
    }
}
