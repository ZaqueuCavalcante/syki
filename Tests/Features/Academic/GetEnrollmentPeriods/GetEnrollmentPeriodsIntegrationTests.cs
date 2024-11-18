namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_return_all_enrollment_periods()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();

        AcademicPeriodOut period = await client.CreateAcademicPeriod("2024.1");
        await client.CreateEnrollmentPeriod(period.Id);

        // Act
        var periods = await client.GetEnrollmentPeriods();

        // Assert
        periods.Should().ContainSingle();
    }
}
