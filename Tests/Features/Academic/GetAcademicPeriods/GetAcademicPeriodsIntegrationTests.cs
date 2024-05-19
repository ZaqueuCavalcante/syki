namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Should_return_institution_academic_periods()
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
