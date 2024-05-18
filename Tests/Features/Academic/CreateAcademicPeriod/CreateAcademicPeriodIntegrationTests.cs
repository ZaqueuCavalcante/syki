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

    [Test]
    public async Task Should_return_only_institution_academic_periods()
    {
        // Arrange
        var clientNovaRoma = await _factory.LoggedAsAcademic();
        var clientUfpe = await _factory.LoggedAsAcademic();

        var period = await clientNovaRoma.CreateAcademicPeriod("2023.1");

        await clientUfpe.CreateAcademicPeriod("2023.2");

        // Act
        var periods = await clientNovaRoma.GetAcademicPeriods();

        // Assert
        periods.Should().HaveCount(1);
        periods[0].Id.Should().Be(period.Id);
    }
}
