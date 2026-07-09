namespace Estud.Tests.Integration;

public partial class IntegrationTests
{
    #region Authentication

    [Test]
    public async Task CourseOfferings_GetCourseOfferings_Should_not_get_offerings_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.GetCourseOfferings();

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Authorization

    [Test]
    public async Task CourseOfferings_GetCourseOfferings_Should_not_get_offerings_when_user_has_no_permission()
    {
        // Arrange
        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.GetCourseOfferings();

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task CourseOfferings_GetCourseOfferings_Should_return_empty_list()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.GetCourseOfferings();

        // Assert
        var offerings = result.Success;
        offerings.Total.Should().Be(0);
        offerings.Items.Should().BeEmpty();
    }

    [Test]
    public async Task CourseOfferings_GetCourseOfferings_Should_return_offerings()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var campus = (await client.CreateCampus()).Success;
        var course = (await client.CreateCourse()).Success;
        var curriculum = (await client.CreateCourseCurriculum(course.Id)).Success;
        var period = (await client.CreateAcademicPeriod("2024.1")).Success;
        await client.CreateCourseOffering(campus.Id, course.Id, curriculum.Id, period.Id);

        // Act
        var result = await client.GetCourseOfferings();

        // Assert
        var offerings = result.Success;
        offerings.Total.Should().Be(1);
        offerings.Items[0].Period.Should().Be("2024.1");
    }

    #endregion
}
