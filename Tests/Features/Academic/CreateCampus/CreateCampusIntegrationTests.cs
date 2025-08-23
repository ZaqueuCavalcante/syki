namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_create_campus()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();

        // Act
        CreateCampusOut campus = await client.CreateCampus("Agreste I", BrazilState.PE, "Caruaru", 123);

        // Assert
        campus.Id.Should().NotBeEmpty();
        campus.Name.Should().Be("Agreste I");
        campus.State.Should().Be(BrazilState.PE);
        campus.City.Should().Be("Caruaru");
        campus.Capacity.Should().Be(123);
    }

    [Test]
    [TestCase("")]
    [TestCase(TestStrings.S51)]
    public async Task Should_not_create_campus_with_invalid_name(string name)
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();

        // Act
        var response = await client.CreateCampus(name, BrazilState.PE, "Caruaru", 123);

        // Assert
        response.ShouldBeError(InvalidCampusName.I);
    }

    [Test]
    [TestCase(null)]
    [TestCase((BrazilState)69)]
    public async Task Should_not_create_campus_with_invalid_brazil_state(BrazilState? state)
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();

        // Act
        var response = await client.CreateCampus("Agreste", state, "Caruaru", 123);

        // Assert
        response.ShouldBeError(InvalidBrazilState.I);
    }

    [Test]
    [TestCase("")]
    [TestCase(TestStrings.S51)]
    public async Task Should_not_create_campus_with_invalid_city(string city)
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();

        // Act
        var response = await client.CreateCampus("Agreste", BrazilState.PE, city, 123);

        // Assert
        response.ShouldBeError(InvalidCampusCity.I);
    }

    [Test]
    [TestCase(0)]
    [TestCase(-1)]
    public async Task Should_not_create_campus_with_invalid_capacity(int capacity)
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();

        // Act
        var response = await client.CreateCampus("Agreste", BrazilState.PE, "Caruaru", capacity);

        // Assert
        response.ShouldBeError(InvalidCampusCapacity.I);
    }
}
