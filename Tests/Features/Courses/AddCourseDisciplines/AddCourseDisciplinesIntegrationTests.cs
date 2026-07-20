namespace Estud.Tests.Integration;

public partial class IntegrationTests
{
    #region Authentication

    [Test]
    public async Task Courses_AddCourseDisciplines_Should_not_add_disciplines_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.AddCourseDisciplines(1, []);

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Authorization

    [Test]
    public async Task Courses_AddCourseDisciplines_Should_not_add_disciplines_when_user_has_no_permission()
    {
        // Arrange
        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.AddCourseDisciplines(1, []);

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    #endregion

    #region Validation errors

    [Test]
    public async Task Courses_AddCourseDisciplines_Should_not_add_disciplines_with_invalid_course()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var response = await client.AddCourseDisciplines(0, [1]);

        // Assert
        response.ShouldBeError(CourseNotFound.I);
    }

    [Test]
    public async Task Courses_AddCourseDisciplines_Should_not_add_disciplines_with_invalid_disciplines_list()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var response = await client.AddCourseDisciplines(1, []);

        // Assert
        response.ShouldBeError(InvalidDisciplinesList.I);
    }

    [Test]
    public async Task Courses_AddCourseDisciplines_Should_not_add_disciplines_with_a_nonexistent_discipline()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        var course = await client.CreateCourse().Success();
        var discipline = await client.CreateDiscipline().Success();

        // Act
        var response = await client.AddCourseDisciplines(course.Id, [discipline.Id, 999999999]);

        // Assert
        response.ShouldBeError(InvalidDisciplinesList.I);
    }

    [Test]
    public async Task Courses_AddCourseDisciplines_Should_not_add_disciplines_with_an_other_institution_discipline()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        var course = await client.CreateCourse().Success();
        var discipline = await client.CreateDiscipline().Success();

        var otherClient = await _back.LoggedAsDirector();
        var otherDiscipline = await otherClient.CreateDiscipline().Success();

        // Act
        var response = await client.AddCourseDisciplines(course.Id, [discipline.Id, otherDiscipline.Id]);

        // Assert
        response.ShouldBeError(InvalidDisciplinesList.I);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task Courses_AddCourseDisciplines_Should_add_disciplines_to_course()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var course = await client.CreateCourse("Análise e Desenvolvimento de Sistemas", CourseType.Tecnologo);
        var discipline = await client.CreateDiscipline("Programação");

        // Act
        var result = await client.AddCourseDisciplines(course.Success.Id, [discipline.Success.Id]);

        // Assert
        result.ShouldBeSuccess();
    }

    #endregion
}
