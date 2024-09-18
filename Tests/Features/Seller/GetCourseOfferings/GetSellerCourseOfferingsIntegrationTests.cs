namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_return_all_seller_course_offerings()
    {
        // Arrange
        var academicClient = await _back.LoggedAsAcademic();
        await academicClient.CreateBasicInstitutionData();

        // Act
        var courseOfferings = await academicClient.GetCourseOfferings();

        // Assert
        courseOfferings.Count.Should().Be(2);
    }
}
