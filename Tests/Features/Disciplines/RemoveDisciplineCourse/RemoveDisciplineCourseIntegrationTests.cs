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
        var course = await client.CreateCourse().Success();

        // Act
        var result = await client.RemoveDisciplineCourse(99999, course.Id);

        // Assert
        result.ShouldBeError(DisciplineNotFound.I);
    }

    [Test]
    public async Task Disciplines_RemoveDisciplineCourse_Should_not_remove_course_not_linked_to_discipline()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var discipline = await client.CreateDiscipline().Success();
        var course = await client.CreateCourse().Success();

        // Act
        var result = await client.RemoveDisciplineCourse(discipline.Id, course.Id);

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
        var discipline = await client.CreateDiscipline().Success();
        var course = await client.CreateCourse().Success();
        await client.AddDisciplineCourses(discipline.Id, [course.Id]);

        // Act
        var result = await client.RemoveDisciplineCourse(discipline.Id, course.Id);

        // Assert
        result.ShouldBeSuccess();
    }

    #endregion
}
