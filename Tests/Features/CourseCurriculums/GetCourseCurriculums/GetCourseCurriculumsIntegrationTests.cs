namespace Estud.Tests.Integration;

public partial class IntegrationTests
{
    #region Authentication

    [Test]
    public async Task CourseCurriculums_GetCourseCurriculums_Should_not_get_course_curriculums_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.GetCourseCurriculums();

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Authorization

    [Test]
    public async Task CourseCurriculums_GetCourseCurriculums_Should_not_get_course_curriculums_when_user_has_no_permission()
    {
        // Arrange
        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.GetCourseCurriculums();

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task CourseCurriculums_GetCourseCurriculums_Should_return_empty_list()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.GetCourseCurriculums();

        // Assert
        result.Success.Total.Should().Be(0);
        result.Success.Items.Should().BeEmpty();
    }

    [Test]
    public async Task CourseCurriculums_GetCourseCurriculums_Should_return_curriculums()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var course = (await client.CreateCourse()).Success;
        await client.CreateCourseCurriculum(course.Id, "Grade 2024");

        // Act
        var result = await client.GetCourseCurriculums();

        // Assert
        result.Success.Total.Should().Be(1);
        result.Success.Items[0].Name.Should().Be("Grade 2024");
        result.Success.Items[0].CourseId.Should().Be(course.Id);
    }

    #endregion
}
