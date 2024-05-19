namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Should_return_all_enrollment_periods()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademic();

        var period = await client.CreateAcademicPeriod("2024.1");
        await client.CreateEnrollmentPeriod(period.Id, "15/01", "28/01");

        // Act
        var periods = await client.GetEnrollmentPeriods();

        // Assert
        periods.Should().ContainSingle();
    }
}
