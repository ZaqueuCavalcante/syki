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
        CampusOut updatedCampus = await client.UpdateCampus(campus.Id, "Agreste II", BrazilState.PE, "Bonito", 789);

        // Assert
        updatedCampus.Id.Should().Be(campus.Id);
        updatedCampus.Name.Should().Be("Agreste II");
        updatedCampus.State.Should().Be(BrazilState.PE);
        updatedCampus.City.Should().Be("Bonito");
        updatedCampus.Capacity.Should().Be(789);
    }

    [Test]
    public async Task Should_not_update_random_campus()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();
        await client.CreateCampus();

        // Act
        var response = await client.UpdateCampus(Guid.CreateVersion7(), "Agreste II", BrazilState.PE, "Bonito");

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
