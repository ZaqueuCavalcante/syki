namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_get_empty_list_when_has_no_campi()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();

        // Act
        var campi = await client.GetCampi();

        // Assert
        campi.Should().BeEmpty();
    }

    [Test]
    public async Task Should_get_many_campus()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();

        await client.CreateCampus("Agreste I", BrazilState.PE, "Caruaru");
        await client.CreateCampus("Suassuna I", BrazilState.PE, "Recife");

        // Act
        var campi = await client.GetCampi();

        // Assert
        campi.Should().HaveCount(2);
    }

    [Test]
    public async Task Should_get_only_institution_campus()
    {
        // Arrange
        var novaRoma = await _api.LoggedAsAcademic();
        var ufpe = await _api.LoggedAsAcademic();

        await novaRoma.CreateCampus("Agreste I", BrazilState.PE, "Caruaru");
        await ufpe.CreateCampus("Suassuna I", BrazilState.PE, "Recife");

        // Act
        var campi = await novaRoma.GetCampi();

        // Assert
        campi.Should().HaveCount(1);
        campi[0].Name.Should().Be("Agreste I");
    }
}
