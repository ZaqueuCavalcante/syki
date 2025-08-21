namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_update_a_campus()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();
        CampusOut campus = await client.CreateCampus();

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
    [TestCase("")]
    [TestCase(TestStrings.S51)]
    public async Task Should_not_update_campus_with_invalid_name(string name)
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();
        CampusOut campus = await client.CreateCampus();

        // Act
        var response = await client.UpdateCampus(campus.Id, name, BrazilState.PE, "Bonito");

        // Assert
        response.ShouldBeError(InvalidCampusName.I);
    }

    [Test]
    [TestCase(null)]
    [TestCase((BrazilState)69)]
    public async Task Should_not_update_campus_with_invalid_brazil_state(BrazilState? state)
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();
        CampusOut campus = await client.CreateCampus();

        // Act
        var response = await client.UpdateCampus(campus.Id, "Agreste II", state, "Bonito");

        // Assert
        response.ShouldBeError(InvalidBrazilState.I);
    }

    [Test]
    [TestCase("")]
    [TestCase(TestStrings.S51)]
    public async Task Should_not_update_campus_with_invalid_city(string city)
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();
        CampusOut campus = await client.CreateCampus();

        // Act
        var response = await client.UpdateCampus(campus.Id, "Agreste II", BrazilState.PE, city);

        // Assert
        response.ShouldBeError(InvalidCampusCity.I);
    }

    [Test]
    [TestCase(0)]
    [TestCase(-1)]
    public async Task Should_not_update_campus_with_invalid_capacity(int capacity)
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();
        CampusOut campus = await client.CreateCampus();

        // Act
        var response = await client.UpdateCampus(campus.Id, "Agreste II", BrazilState.PE, "Bonito", capacity);

        // Assert
        response.ShouldBeError(InvalidCampusCapacity.I);
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
        response.ShouldBeError(CampusNotFound.I); 
    }

    [Test]
    public async Task Should_not_update_other_institution_campus()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();
        await client.CreateCampus();

        var otherClient = await _api.LoggedAsAcademic();
        CampusOut otherCampus = await otherClient.CreateCampus();
        
        // Act
        var response = await client.UpdateCampus(otherCampus.Id, "Agreste II", BrazilState.PE, "Bonito");

        // Assert
        response.ShouldBeError(CampusNotFound.I); 
    }
}
