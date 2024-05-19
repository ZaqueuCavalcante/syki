namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Should_create_academic_period()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademic();

        // Act
        var period = await client.CreateAcademicPeriod("2024.1");

        // Assert
        period.Id.Should().Be(period.Id);
    }

    [Test]
    public async Task Should_create_many_academic_periods()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademic();

        // Act
        await client.CreateAcademicPeriod("2023.1");
        await client.CreateAcademicPeriod("2023.2");

        // Assert
        var periods = await client.GetAcademicPeriods();
        periods.Should().HaveCount(2);
    }
}
