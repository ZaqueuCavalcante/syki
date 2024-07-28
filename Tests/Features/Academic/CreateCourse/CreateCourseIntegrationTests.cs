using static Syki.Shared.CourseType;

namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_create_a_new_course()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();

        // Act
        var course = await client.CreateCourse("Análise e Desenvolvimento de Sistemas", Bacharelado);

        // Assert
        course.Id.Should().NotBeEmpty();
        course.Name.Should().Be("Análise e Desenvolvimento de Sistemas");
        course.Type.Should().Be(Bacharelado);
    }

    [Test]
    public async Task Should_not_create_a_new_course_with_invalid_type()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();

        // Act
        var (_, response) = await client.CreateCourseTuple("Análise e Desenvolvimento de Sistemas", (CourseType)69);

        // Assert
        await response.AssertBadRequest(new InvalidCourseType());
    }
}
