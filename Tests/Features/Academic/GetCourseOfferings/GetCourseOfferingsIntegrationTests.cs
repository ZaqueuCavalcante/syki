namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_return_all_course_offerings()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();
        await client.CreateBasicInstitutionData();

        // Act
        var courseOfferings = await client.GetCourseOfferings();

        // Assert
        courseOfferings.Count.Should().Be(1);
    }

    [Test]
    public async Task Should_not_return_any_course_offering()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();

        // Act
        var courseOfferings = await client.GetCourseOfferings();

        // Assert
        courseOfferings.Should().BeEmpty();
    }
}
