namespace Estud.Tests.Integration;

public partial class IntegrationTests
{
    #region Authentication

    [Test]
    public async Task Disciplines_RemoveDisciplineCourse_Should_not_remove_discipline_course_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.RemoveDisciplineCourse(1, 1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Authorization

    [Test]
    public async Task Disciplines_RemoveDisciplineCourse_Should_not_remove_discipline_course_when_user_has_no_permission()
    {
        // Arrange
        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.RemoveDisciplineCourse(1, 1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    #endregion

    #region Validation errors

    [Test]
    public async Task Disciplines_RemoveDisciplineCourse_Should_not_remove_course_from_discipline_not_found()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var course = await client.CreateCourse();

        // Act
        var result = await client.RemoveDisciplineCourse(99999, course.Success.Id);

        // Assert
        result.ShouldBeError(DisciplineNotFound.I);
    }

    [Test]
    public async Task Disciplines_RemoveDisciplineCourse_Should_not_remove_course_not_linked_to_discipline()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var discipline = await client.CreateDiscipline();
        var course = await client.CreateCourse();

        // Act
        var result = await client.RemoveDisciplineCourse(discipline.Success.Id, course.Success.Id);

        // Assert
        result.ShouldBeError(CourseDisciplineNotFound.I);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task Disciplines_RemoveDisciplineCourse_Should_remove_discipline_course()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var discipline = await client.CreateDiscipline();
        var course = await client.CreateCourse();
        await client.AddDisciplineCourses(discipline.Success.Id, [course.Success.Id]);

        // Act
        var result = await client.RemoveDisciplineCourse(discipline.Success.Id, course.Success.Id);

        // Assert
        result.ShouldBeSuccess();
    }

    #endregion
}
