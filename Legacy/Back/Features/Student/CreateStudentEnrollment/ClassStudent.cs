namespace Syki.Back.Features.Student.CreateStudentEnrollment;

/// <summary>
/// Vínculo entre Aluno e Turma
/// </summary>
public class ClassStudent
{
    public Guid ClassId { get; set; }
    public Guid SykiStudentId { get; set; }
    public StudentDisciplineStatus StudentDisciplineStatus { get; set; }

    private ClassStudent() { }

    public ClassStudent(
        Guid classId,
        Guid studentId
    ) {
        ClassId = classId;
        SykiStudentId = studentId;
        StudentDisciplineStatus = StudentDisciplineStatus.Matriculado;
    }
}
