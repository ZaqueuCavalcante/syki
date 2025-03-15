namespace Syki.Back.Features.Teacher.CreateClassActivity;

/// <summary>
/// Nota de uma Atividade feita por um Aluno dentro de uma Turma
/// </summary>
public class StudentClassActivityNote : Entity
{
    public Guid Id { get; }
    public Guid StudentId { get; }
    public Guid ClassActivityId { get; set; }

    /// <summary>
    /// Nota no intervalo: 0.00 ≤ Value ≤ 10.00
    /// </summary>
    public decimal Value { get; set; }

    public DateTime CreatedAt { get; set; }

    private StudentClassActivityNote() {}

    public StudentClassActivityNote(
        Guid studentId,
        Guid classActivityId,
        decimal value
    ) {
        Id = Guid.NewGuid();
        StudentId = studentId;
        ClassActivityId = classActivityId;
        Value = value;
        CreatedAt = DateTime.Now;
    }
}
