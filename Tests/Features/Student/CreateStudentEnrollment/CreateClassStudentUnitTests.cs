using Syki.Back.Features.Student.CreateStudentEnrollment;

namespace Syki.Tests.Features.Student.CreateStudentEnrollment;

public class CreateClassStudentUnitTests
{
    [Test]
    public void Should_create_class_student_with_correct_data()
    {
        // Arrange
        var classId = Guid.NewGuid();
        var studentId = Guid.NewGuid();
        var status = StudentDisciplineStatus.Matriculado;

        // Act
        var student = new ClassStudent(classId, studentId);

        // Assert
        student.ClassId.Should().Be(classId);
        student.SykiStudentId.Should().Be(studentId);
        student.StudentDisciplineStatus.Should().Be(status);
    }
}
