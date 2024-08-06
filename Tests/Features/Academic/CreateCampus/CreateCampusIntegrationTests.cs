namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_create_campus()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();

        // Act
        var campus = await client.CreateCampus("Agreste I", "Caruaru - PE");

        // Assert
        campus.Id.Should().NotBeEmpty();
        campus.Name.Should().Be("Agreste I");
        campus.City.Should().Be("Caruaru - PE");
    }
}
