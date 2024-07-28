namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_update_a_campus()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();
        var campus = await client.CreateCampus();

        // Act
        var updatedCampus = await client.UpdateCampus(campus.Id, "Agreste II", "Bonito - PE");

        // Assert
        updatedCampus.Id.Should().Be(campus.Id);
        updatedCampus.Name.Should().Be("Agreste II");
        updatedCampus.City.Should().Be("Bonito - PE");
    }

    [Test]
    public async Task Should_not_update_random_campus()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();
        await client.CreateCampus();

        // Act
        var response = await client.UpdateCampusHttp(Guid.NewGuid(), "Agreste II", "Bonito - PE");

        // Assert
        await response.AssertBadRequest(new CampusNotFound()); 
    }

    [Test]
    public async Task Should_not_update_other_institution_campus()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();
        await client.CreateCampus();

        var otherClient = await _back.LoggedAsAcademic();
        var otherCampus = await otherClient.CreateCampus();
        
        // Act
        var response = await client.UpdateCampusHttp(otherCampus.Id, "Agreste II", "Bonito - PE");

        // Assert
        await response.AssertBadRequest(new CampusNotFound()); 
    }
}
