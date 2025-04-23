namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_create_campus()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();

        // Act
        var campus = await client.CreateCampus("Agreste I", BrazilState.PE, "Caruaru");

        // Assert
        campus.Id.Should().NotBeEmpty();
        campus.Name.Should().Be("Agreste I");
        campus.State.Should().Be(BrazilState.PE);
        campus.City.Should().Be("Caruaru");
    }
}
