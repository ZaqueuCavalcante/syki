namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Deve_atualizar_um_campus()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademico();
        var campus = await client.CreateCampus("Agreste I", "Caruaru - PE");

        // Act
        var updatedCampus = await client.UpdateCampus(campus.Id, "Agreste II", "Bonito - PE");

        // Assert
        updatedCampus.Id.Should().Be(campus.Id);
        updatedCampus.Name.Should().Be("Agreste II");
        updatedCampus.City.Should().Be("Bonito - PE");
    }
}
