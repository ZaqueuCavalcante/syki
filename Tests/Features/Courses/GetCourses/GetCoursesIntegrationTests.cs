namespace Estud.Tests.Integration;

public partial class IntegrationTests
{
    #region Authentication

    [Test]
    public async Task Courses_GetCourses_Should_not_get_courses_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.GetCourses();

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Authorization

    [Test]
    public async Task Courses_GetCourses_Should_not_get_courses_when_user_has_no_permission()
    {
        // Arrange
        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.GetCourses();

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    #endregion

    #region Happy path

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
        result.Success.Total.Should().Be(2);
        result.Success.Items.First().Name.Should().Be("Direito");
        result.Success.Items.Last().Name.Should().Be("Pedagogia");
    }

    #endregion
}
