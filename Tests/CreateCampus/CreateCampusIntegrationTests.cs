namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    // [Test]
    public async Task Deve_criar_um_novo_campus()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademico();

        // Act
        var campus = await client.NewCampus("Agreste I", "Caruaru - PE");

        // Assert
        campus.Id.Should().NotBeEmpty();
        campus.Name.Should().Be("Agreste I");
        campus.City.Should().Be("Caruaru - PE");
    }
}
