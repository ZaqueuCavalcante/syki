namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Campi_GetCampi_Should_get_campi()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        await client.CreateCampus("Suassuna", BrazilState.PE, "Recife", 150);
        await client.CreateCampus("Agreste", BrazilState.PE, "Caruaru", 120);

        // Act
        var result = await client.GetCampi();

        // Assert
        result.Total.Should().Be(2);
        result.Items.First().Name.Should().Be("Agreste");
        result.Items.Last().Name.Should().Be("Suassuna");
    }
}
