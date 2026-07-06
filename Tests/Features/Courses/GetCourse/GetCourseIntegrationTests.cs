namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    #region Authentication

    [Test]
    public async Task Courses_GetCourse_Should_not_get_course_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.GetCourse(1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Authorization

    [Test]
    public async Task Courses_GetCourse_Should_not_get_course_when_user_has_no_permission()
    {
        // Arrange
        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.GetCourse(1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    #endregion

    #region Validation errors

    [Test]
    public async Task Courses_GetCourse_Should_not_get_course_not_found()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.GetCourse(99999);

        // Assert
        result.ShouldBeError(CourseNotFound.I);
    }

    [Test]
    public async Task Courses_GetCourse_Should_not_get_other_institution_course()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        var otherClient = await _back.LoggedAsDirector();
        var otherCourse = await otherClient.CreateCourse();

        // Act
        var result = await client.GetCourse(otherCourse.Success.Id);

        // Assert
        result.ShouldBeError(CourseNotFound.I);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task Courses_GetCourse_Should_get_course()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var course = await client.CreateCourse("Análise e Desenvolvimento de Sistemas", CourseType.Tecnologo);

        // Act
        var result = await client.GetCourse(course.Success.Id);

        // Assert
        result.Success.Id.Should().Be(course.Success.Id);
        result.Success.Name.Should().Be("Análise e Desenvolvimento de Sistemas");
        result.Success.Type.Should().Be("Tecnólogo");
        result.Success.Disciplines.Should().BeEmpty();
    }

    [Test]
    public async Task Courses_GetCourse_Should_get_course_with_disciplines()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var course = await client.CreateCourse();
        var calculo = await client.CreateDiscipline("Cálculo I");
        var geometria = await client.CreateDiscipline("Geometria");
        await client.AddCourseDisciplines(course.Success.Id, [calculo.Success.Id, geometria.Success.Id]);

        // Act
        var result = await client.GetCourse(course.Success.Id);

        // Assert
        result.Success.Disciplines.Should().HaveCount(2);
        result.Success.Disciplines.First().Name.Should().Be("Cálculo I");
        result.Success.Disciplines.Last().Name.Should().Be("Geometria");
    }

    #endregion
}
