using Estud.Back.Features.Student.CreateStudentEnrollment;

namespace Estud.Tests.Features.Student.CreateStudentEnrollment;

public class CreateClassStudentUnitTests
{
    // [Test]
    public void Should_create_class_student_with_correct_data()
    {
        // Arrange
        var classId = Guid.CreateVersion7();
        var studentId = Guid.CreateVersion7();
        var status = StudentDisciplineStatus.Matriculado;

        // Act
        var student = new ClassStudent(classId, studentId);

        // Assert
        student.ClassId.Should().Be(classId);
        student.EstudStudentId.Should().Be(studentId);
        student.StudentDisciplineStatus.Should().Be(status);
    }
}
