namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task CourseOfferings_GetCourseOfferings_Should_return_empty_list()
    {
        // Arrange
        var client = await _back.LoggedAsDirectot();

        // Act
        var result = await client.GetCourseOfferings();

        // Assert
        result.Total.Should().Be(0);
        result.Items.Should().BeEmpty();
    }

    [Test]
    public async Task CourseOfferings_GetCourseOfferings_Should_return_offerings()
    {
        // Arrange
        var client = await _back.LoggedAsDirectot();
        var campus = (await client.CreateCampus()).Success;
        var course = (await client.CreateCourse()).Success;
        var curriculum = (await client.CreateCourseCurriculum(course.Id)).Success;
        var period = (await client.CreateAcademicPeriod("2024.1")).Success;
        await client.CreateCourseOffering(campus.Id, course.Id, curriculum.Id, period.Id);

        // Act
        var result = await client.GetCourseOfferings();

        // Assert
        result.Total.Should().Be(1);
        result.Items[0].Period.Should().Be("2024.1");
    }
}
