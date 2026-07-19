using Estud.Back.Domain.Students;

namespace Estud.Back.Domain.Classes;

/// <summary>
/// Vínculo entre Turma e Aluno
/// </summary>
public class ClassStudent
{
    public int ClassId { get; set; }
    public int StudentId { get; set; }
    public StudentClassStatus Status { get; set; }

    public EstudStudent? Student { get; set; }

    private ClassStudent() { }

    public ClassStudent(
        int classId,
        int studentId
    ) {
        ClassId = classId;
        StudentId = studentId;
        Status = StudentClassStatus.Matriculado;
    }
}
