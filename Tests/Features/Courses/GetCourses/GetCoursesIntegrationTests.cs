namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Courses_GetCourses_Should_get_courses()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        await client.CreateCourse("Pedagogia", CourseType.Licenciatura);
        await client.CreateCourse("Direito", CourseType.Bacharelado);

        // Act
        var result = await client.GetCourses();

        // Assert
        result.Total.Should().Be(2);
        result.Items.First().Name.Should().Be("Direito");
        result.Items.Last().Name.Should().Be("Pedagogia");
    }
}
