namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Should_create_enrollment_period()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademic();
        var period = await client.CreateAcademicPeriod("2024.1");

        // Act
        var enrollmentPeriod = await client.CreateEnrollmentPeriod(period.Id, "15/01", "28/01");

        // Assert
        enrollmentPeriod.Id.Should().NotBeEmpty();
    }
}
