namespace Estud.Tests.Integration;

public partial class IntegrationTests
{
    #region Authentication

    [Test]
    public async Task Courses_CreateCourse_Should_not_create_course_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.CreateCourse();

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Authorization

    [Test]
    public async Task Courses_CreateCourse_Should_not_create_course_when_user_has_no_permission()
    {
        // Arrange
        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.CreateCourse();

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    #endregion

    #region Validation errors

    [Test]
    [TestCase("")]
    public async Task Courses_CreateCourse_Should_not_create_course_with_invalid_name(string name)
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var response = await client.CreateCourse(name, CourseType.Tecnologo);

        // Assert
        response.ShouldBeError(InvalidCourseName.I);
    }

    [Test]
    [TestCase(null)]
    [TestCase((CourseType)69)]
    public async Task Courses_CreateCourse_Should_not_create_course_with_invalid_type(CourseType? type)
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var response = await client.CreateCourse("Análise e Desenvolvimento de Sistemas", type);

        // Assert
        response.ShouldBeError(InvalidCourseType.I);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task Courses_CreateCourse_Should_create_course()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.CreateCourse("Análise e Desenvolvimento de Sistemas", CourseType.Tecnologo);

        // Assert
        var course = result.Success;
        course.Id.Should().NotBe(0);
    }

    #endregion
}
