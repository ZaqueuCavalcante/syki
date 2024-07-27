namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_create_academic_period()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();

        // Act
        var period = await client.CreateAcademicPeriod("2024.1");

        // Assert
        period.Id.Should().Be(period.Id);
    }

    [Test]
    public async Task Should_create_many_academic_periods()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();

        // Act
        await client.CreateAcademicPeriod("2023.1");
        await client.CreateAcademicPeriod("2023.2");

        // Assert
        var periods = await client.GetAcademicPeriods();
        periods.Should().HaveCount(2);
    }

    [Test]
    public async Task Should_not_create_duplicated_academic_period()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();

        // Act
        await client.CreateAcademicPeriod("2024.1");
        var response = await client.CreateAcademicPeriodHttp("2024.1");

        // Assert
        await response.AssertBadRequest(new AcademicPeriodAlreadyExists());
    }
}
