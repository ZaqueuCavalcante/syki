namespace Estud.Tests.Integration;

public partial class IntegrationTests
{
    #region Authentication

    [Test]
    public async Task Courses_GetCourseDisciplines_Should_not_get_course_disciplines_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.GetCourseDisciplines(1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Authorization

    [Test]
    public async Task Courses_GetCourseDisciplines_Should_not_get_course_disciplines_when_user_has_no_permission()
    {
        // Arrange
        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.GetCourseDisciplines(1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task Courses_GetCourseDisciplines_Should_get_empty_course_disciplines()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var course = await client.CreateCourse().Success();

        // Act
        var result = await client.GetCourseDisciplines(course.Id);

        // Assert
        result.Success.Total.Should().Be(0);
        result.Success.Items.Should().BeEmpty();
    }

    [Test]
    public async Task Courses_GetCourseDisciplines_Should_get_course_disciplines()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var course = await client.CreateCourse().Success();
        var discipline1 = await client.CreateDiscipline("Geometria").Success();
        var discipline2 = await client.CreateDiscipline("Álgebra").Success();
        await client.AddCourseDisciplines(course.Id, [discipline1.Id, discipline2.Id]);

        // Act
        var result = await client.GetCourseDisciplines(course.Id);

        // Assert
        result.Success.Total.Should().Be(2);
        result.Success.Items.First().Name.Should().Be("Álgebra");
        result.Success.Items.Last().Name.Should().Be("Geometria");
    }

    #endregion
}
