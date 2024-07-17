namespace Syki.Back.Features.Student.CreateStudentEnrollment;

public class ClassStudent
{
    public Guid ClassId { get; set; }
    public Guid SykiStudentId { get; set; }
    public StudentDisciplineStatus StudentDisciplineStatus { get; set; }

    public ClassStudent() {}

    public ClassStudent(
        Guid classId,
        Guid studentId,
        StudentDisciplineStatus status
    ) {
        ClassId = classId;
        SykiStudentId = studentId;
        StudentDisciplineStatus = status;
    }
}
