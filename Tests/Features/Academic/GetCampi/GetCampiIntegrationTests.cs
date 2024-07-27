namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_get_empty_list_when_has_no_campi()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();

        // Act
        var campi = await client.GetCampi();

        // Assert
        campi.Should().BeEmpty();
    }

    [Test]
    public async Task Should_get_many_campus()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();

        await client.CreateCampus("Agreste I", "Caruaru - PE");
        await client.CreateCampus("Suassuna I", "Recife - PE");

        // Act
        var campi = await client.GetCampi();

        // Assert
        campi.Should().HaveCount(2);
    }

    [Test]
    public async Task Should_get_only_institution_campus()
    {
        // Arrange
        var novaRoma = await _back.LoggedAsAcademic();
        var ufpe = await _back.LoggedAsAcademic();

        await novaRoma.CreateCampus("Agreste I", "Caruaru - PE");
        await ufpe.CreateCampus("Suassuna I", "Recife - PE");

        // Act
        var campi = await novaRoma.GetCampi();

        // Assert
        campi.Should().HaveCount(1);
        campi[0].Name.Should().Be("Agreste I");
    }
}
