namespace Syki.Back.Features.Student.CreateStudentEnrollment;

public class ClassStudent
{
    public Guid ClassId { get; set; }
    public Guid SykiStudentId { get; set; }
    public StudentDisciplineStatus StudentDisciplineStatus { get; set; }

    // Only for EF
    public ClassStudent() { }

    public ClassStudent(
        Guid classId,
        Guid studentId
    ) {
        ClassId = classId;
        SykiStudentId = studentId;
        StudentDisciplineStatus = StudentDisciplineStatus.Matriculado;
    }
}
