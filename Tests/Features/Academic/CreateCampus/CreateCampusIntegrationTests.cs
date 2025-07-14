namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_create_campus()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();

        // Act
        var campus = await client.CreateCampus("Agreste I", BrazilState.PE, "Caruaru", 123);

        // Assert
        campus.Id.Should().NotBeEmpty();
        campus.Name.Should().Be("Agreste I");
        campus.State.Should().Be(BrazilState.PE);
        campus.City.Should().Be("Caruaru");
        campus.Capacity.Should().Be(123);
        campus.Students.Should().Be(0);
        campus.FillRate.Should().Be(0);
    }
}
