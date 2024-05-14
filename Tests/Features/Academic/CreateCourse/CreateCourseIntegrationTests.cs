using static Syki.Shared.CourseType;

namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Should_create_a_new_course()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademic();

        // Act
        var course = await client.CreateCourse("Análise e Desenvolvimento de Sistemas", Bacharelado);

        // Assert
        course.Id.Should().NotBeEmpty();
        course.Name.Should().Be("Análise e Desenvolvimento de Sistemas");
        course.Type.Should().Be(Bacharelado);
    }

    [Test]
    public async Task Should_create_many_courses()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademic();

        // Act
        await client.CreateCourse("Análise e Desenvolvimento de Sistemas", Bacharelado);
        await client.CreateCourse("Direito", Licenciatura);

        // Assert
        var courses = await client.GetCourses();
        courses.Should().HaveCount(2);
    }
}
