namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    #region Authentication

    [Test]
    public async Task Disciplines_AddDisciplineCourses_Should_not_add_discipline_courses_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.AddDisciplineCourses(1, [1]);

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Authorization

    [Test]
    public async Task Disciplines_AddDisciplineCourses_Should_not_add_discipline_courses_when_user_has_no_permission()
    {
        // Arrange
        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.AddDisciplineCourses(1, [1]);

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    #endregion

    #region Validation errors

    [Test]
    public async Task Disciplines_AddDisciplineCourses_Should_not_add_discipline_courses_with_empty_courses_list()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var discipline = await client.CreateDiscipline();

        // Act
        var response = await client.AddDisciplineCourses(discipline.Success.Id, []);

        // Assert
        response.ShouldBeError(InvalidCoursesList.I);
    }

    [Test]
    public async Task Disciplines_AddDisciplineCourses_Should_not_add_discipline_courses_with_duplicate_courses()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var discipline = await client.CreateDiscipline();

        // Act
        var response = await client.AddDisciplineCourses(discipline.Success.Id, [1, 1]);

        // Assert
        response.ShouldBeError(InvalidCoursesList.I);
    }

    [Test]
    public async Task Disciplines_AddDisciplineCourses_Should_not_add_courses_to_discipline_not_found()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var course = await client.CreateCourse();

        // Act
        var response = await client.AddDisciplineCourses(99999, [course.Success.Id]);

        // Assert
        response.ShouldBeError(DisciplineNotFound.I);
    }

    [Test]
    public async Task Disciplines_AddDisciplineCourses_Should_not_add_courses_from_other_institution()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var discipline = await client.CreateDiscipline();

        var otherClient = await _back.LoggedAsDirector();
        var otherCourse = await otherClient.CreateCourse();

        // Act
        var response = await client.AddDisciplineCourses(discipline.Success.Id, [otherCourse.Success.Id]);

        // Assert
        response.ShouldBeError(InvalidCoursesList.I);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task Disciplines_AddDisciplineCourses_Should_add_discipline_courses()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var discipline = await client.CreateDiscipline();
        var course = await client.CreateCourse();

        // Act
        var result = await client.AddDisciplineCourses(discipline.Success.Id, [course.Success.Id]);

        // Assert
        result.ShouldBeSuccess();
    }

    #endregion
}
