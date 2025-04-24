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
    public async Task Should_get_campi_ordered_by_name()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();

        var campusSuassuna = await client.CreateCampus("Suassuna I", BrazilState.PE, "Recife");
        var campusAgreste = await client.CreateCampus("Agreste I", BrazilState.PE, "Caruaru");

        // Act
        var campi = await client.GetCampi();

        // Assert
        campi.Should().HaveCount(2);

        campi[0].Should().BeEquivalentTo(campusAgreste);
        campi[1].Should().BeEquivalentTo(campusSuassuna);
    }

    [Test]
    public async Task Should_get_only_institution_campus()
    {
        // Arrange
        var clientA = await _api.LoggedAsAcademic();
        var clientB = await _api.LoggedAsAcademic();

        var campusA = await clientA.CreateCampus("Agreste I", BrazilState.PE, "Caruaru");
        await clientB.CreateCampus("Suassuna I", BrazilState.PE, "Recife");

        // Act
        var campi = await clientA.GetCampi();

        // Assert
        campi.Should().BeEquivalentTo([campusA]);
    }
}
