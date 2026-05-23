namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    #region Authentication

    [Test]
    public async Task Courses_UpdateCourse_Should_not_update_course_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.UpdateCourse(1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Authorization

    [Test]
    public async Task Courses_UpdateCourse_Should_not_update_course_when_user_has_no_permission()
    {
        // Arrange
        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.UpdateCourse(1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    #endregion

    #region Validation errors

    [Test]
    [TestCase("")]
    public async Task Courses_UpdateCourse_Should_not_update_course_with_invalid_name(string name)
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var course = await client.CreateCourse();

        // Act
        var response = await client.UpdateCourse(course.Success.Id, name, CourseType.Bacharelado);

        // Assert
        response.ShouldBeError(InvalidCourseName.I);
    }

    [Test]
    [TestCase(null)]
    [TestCase((CourseType)69)]
    public async Task Courses_UpdateCourse_Should_not_update_course_with_invalid_type(CourseType? type)
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var course = await client.CreateCourse();

        // Act
        var response = await client.UpdateCourse(course.Success.Id, "Direito", type);

        // Assert
        response.ShouldBeError(InvalidCourseType.I);
    }

    #endregion

    #region Domain errors

    [Test]
    public async Task Courses_UpdateCourse_Should_not_update_course_not_found()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var response = await client.UpdateCourse(99999);

        // Assert
        response.ShouldBeError(CourseNotFound.I);
    }

    [Test]
    public async Task Courses_UpdateCourse_Should_not_update_other_institution_course()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        var otherClient = await _back.LoggedAsDirector();
        var otherCourse = await otherClient.CreateCourse();

        // Act
        var response = await client.UpdateCourse(otherCourse.Success.Id);

        // Assert
        response.ShouldBeError(CourseNotFound.I);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task Courses_UpdateCourse_Should_update_course()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var course = await client.CreateCourse("Análise e Desenvolvimento de Sistemas", CourseType.Tecnologo);

        // Act
        var result = await client.UpdateCourse(course.Success.Id, "Direito", CourseType.Bacharelado);

        // Assert
        var updated = result.Success;
        updated.Id.Should().Be(course.Success.Id);
        updated.Name.Should().Be("Direito");
        updated.Type.Should().Be(CourseType.Bacharelado);
    }

    #endregion
}
