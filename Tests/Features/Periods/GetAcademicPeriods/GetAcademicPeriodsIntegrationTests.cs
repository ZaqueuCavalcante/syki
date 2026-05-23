namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Periods_GetAcademicPeriods_Should_get_academic_periods()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        await client.CreateAcademicPeriod("2024.1", new DateOnly(2024, 02, 01), new DateOnly(2024, 06, 01));
        await client.CreateAcademicPeriod("2024.2", new DateOnly(2024, 08, 01), new DateOnly(2024, 12, 01));

        // Act
        var result = await client.GetAcademicPeriods();

        // Assert
        result.Total.Should().Be(2);
        result.Items.First().Name.Should().Be("2024.1");
        result.Items.Last().Name.Should().Be("2024.2");
    }
}
