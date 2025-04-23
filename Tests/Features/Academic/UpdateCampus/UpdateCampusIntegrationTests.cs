namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_update_a_campus()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();
        var campus = await client.CreateCampus();

        // Act
        CampusOut updatedCampus = await client.UpdateCampus(campus.Id, "Agreste II", BrazilState.PE, "Bonito");

        // Assert
        updatedCampus.Id.Should().Be(campus.Id);
        updatedCampus.Name.Should().Be("Agreste II");
        updatedCampus.State.Should().Be(BrazilState.PE);
        updatedCampus.City.Should().Be("Bonito");
    }

    [Test]
    public async Task Should_not_update_random_campus()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();
        await client.CreateCampus();

        // Act
        var response = await client.UpdateCampus(Guid.NewGuid(), "Agreste II", BrazilState.PE, "Bonito");

        // Assert
        response.ShouldBeError(new CampusNotFound()); 
    }

    [Test]
    public async Task Should_not_update_other_institution_campus()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();
        await client.CreateCampus();

        var otherClient = await _api.LoggedAsAcademic();
        var otherCampus = await otherClient.CreateCampus();
        
        // Act
        var response = await client.UpdateCampus(otherCampus.Id, "Agreste II", BrazilState.PE, "Bonito");

        // Assert
        response.ShouldBeError(new CampusNotFound()); 
    }
}
