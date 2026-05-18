using Syki.Tests.Integration.Base;

namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    [TestCase("")]
    public async Task Campi_CreateCampus_Should_not_create_campus_with_invalid_name(string name)
    {
        // Arrange
        var client = await _back.LoggedAs($"CreateCampus_{DataGen.Numbers}", [SykiPermissions.ManageCampi]);

        // Act
        var response = await client.CreateCampus(name, BrazilState.PE, "Caruaru", 123);

        // Assert
        response.ShouldBeError(InvalidCampusName.I);
    }

    [Test]
    [TestCase(null)]
    [TestCase((BrazilState)69)]
    public async Task Campi_CreateCampus_Should_not_create_campus_with_invalid_brazil_state(BrazilState? state)
    {
        // Arrange
        var client = await _back.LoggedAs($"CreateCampus_{DataGen.Numbers}", [SykiPermissions.ManageCampi]);

        // Act
        var response = await client.CreateCampus("Agreste", state, "Caruaru", 123);

        // Assert
        response.ShouldBeError(InvalidBrazilState.I);
    }

    [Test]
    [TestCase("")]
    public async Task Campi_CreateCampus_Should_not_create_campus_with_invalid_city(string city)
    {
        // Arrange
        var client = await _back.LoggedAs($"CreateCampus_{DataGen.Numbers}", [SykiPermissions.ManageCampi]);

        // Act
        var response = await client.CreateCampus("Agreste", BrazilState.PE, city, 123);

        // Assert
        response.ShouldBeError(InvalidCampusCity.I);
    }

    [Test]
    [TestCase(0)]
    [TestCase(-1)]
    public async Task Campi_CreateCampus_Should_not_create_campus_with_invalid_capacity(int capacity)
    {
        // Arrange
        var client = await _back.LoggedAs($"CreateCampus_{DataGen.Numbers}", [SykiPermissions.ManageCampi]);

        // Act
        var response = await client.CreateCampus("Agreste", BrazilState.PE, "Caruaru", capacity);

        // Assert
        response.ShouldBeError(InvalidCampusCapacity.I);
    }

    [Test]
    public async Task Campi_CreateCampus_Should_create_campus()
    {
        // Arrange
        var client = await _back.LoggedAs($"CreateCampus_{DataGen.Numbers}", [SykiPermissions.ManageCampi]);

        // Act
        var result = await client.CreateCampus("Agreste I", BrazilState.PE, "Caruaru", 123);

        // Assert
        var campus = result.Success;
        campus.Id.Should().NotBe(0);
    }
}
