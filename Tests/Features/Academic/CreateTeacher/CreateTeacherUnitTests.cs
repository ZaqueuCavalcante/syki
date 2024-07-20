using Syki.Back.Features.Academic.CreateTeacher;

namespace Syki.Tests.Features.Academic.CreateTeacher;

public class CreateTeacherUnitTests
{
    [Test]
    public void Should_create_teacher_with_correct_data()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var institutionId = Guid.NewGuid();
        const string name = "Chico Science";

        // Act
        var teacher = new SykiTeacher(userId, institutionId, name);

        // Assert
        teacher.Id.Should().NotBeEmpty();
        teacher.InstitutionId.Should().Be(institutionId);
        teacher.Id.Should().Be(userId);
        teacher.Name.Should().Be(name);
    }

    [Test]
    public void Should_convert_teacher_to_out()
    {
        // Arrange
        var institutionId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        const string name = "Chico Science";
        var teacher = new SykiTeacher(userId, institutionId, name);

        // Act
        var teacherOut = teacher.ToOut();

        // Assert
        teacherOut.Id.Should().Be(teacher.Id);
        teacherOut.Name.Should().Be(teacher.Name);
    }
}
