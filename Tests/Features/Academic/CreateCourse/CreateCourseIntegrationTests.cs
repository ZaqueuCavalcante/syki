using static Syki.Shared.CourseType;

namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_create_course()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();

        // Act
        CourseOut course = await client.CreateCourse("Análise e Desenvolvimento de Sistemas", Bacharelado);

        // Assert
        course.Id.Should().NotBeEmpty();
        course.Name.Should().Be("Análise e Desenvolvimento de Sistemas");
        course.Type.Should().Be(Bacharelado);
    }

    [Test]
    public async Task Should_not_create_course_with_invalid_type()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();

        // Act
        var response = await client.CreateCourse("Análise e Desenvolvimento de Sistemas", (CourseType)69);

        // Assert
        response.ShouldBeError(new InvalidCourseType());
    }
}
