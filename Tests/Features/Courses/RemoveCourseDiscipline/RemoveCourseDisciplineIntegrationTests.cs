namespace Estud.Tests.Integration;

public partial class IntegrationTests
{
    #region Authentication

    [Test]
    public async Task Courses_RemoveCourseDiscipline_Should_not_remove_course_discipline_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.RemoveCourseDiscipline(1, 1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Authorization

    [Test]
    public async Task Courses_RemoveCourseDiscipline_Should_not_remove_course_discipline_when_user_has_no_permission()
    {
        // Arrange
        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.RemoveCourseDiscipline(1, 1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    #endregion

    #region Validation errors

    [Test]
    public async Task Courses_RemoveCourseDiscipline_Should_not_remove_discipline_from_course_not_found()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var discipline = await client.CreateDiscipline().Success();

        // Act
        var result = await client.RemoveCourseDiscipline(99999, discipline.Id);

        // Assert
        result.ShouldBeError(CourseNotFound.I);
    }

    [Test]
    public async Task Courses_RemoveCourseDiscipline_Should_not_remove_discipline_from_other_institution_course()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        var otherClient = await _back.LoggedAsDirector();
        var otherCourse = await otherClient.CreateCourse().Success();

        // Act
        var result = await client.RemoveCourseDiscipline(otherCourse.Id, 1);

        // Assert
        result.ShouldBeError(CourseNotFound.I);
    }

    [Test]
    public async Task Courses_RemoveCourseDiscipline_Should_not_remove_discipline_not_linked_to_course()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var course = await client.CreateCourse().Success();
        var discipline = await client.CreateDiscipline().Success();

        // Act
        var result = await client.RemoveCourseDiscipline(course.Id, discipline.Id);

        // Assert
        result.ShouldBeError(CourseDisciplineNotFound.I);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task Courses_RemoveCourseDiscipline_Should_remove_course_discipline()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var course = await client.CreateCourse().Success();
        var discipline = await client.CreateDiscipline().Success();
        await client.AddCourseDisciplines(course.Id, [discipline.Id]);

        // Act
        var result = await client.RemoveCourseDiscipline(course.Id, discipline.Id);

        // Assert
        result.ShouldBeSuccess();
    }

    #endregion
}
