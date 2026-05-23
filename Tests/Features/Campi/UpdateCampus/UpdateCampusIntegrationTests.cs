namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    #region Authentication

    [Test]
    public async Task Campi_UpdateCampus_Should_not_update_campus_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.UpdateCampus(1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Authorization

    [Test]
    public async Task Campi_UpdateCampus_Should_not_update_campus_when_analyst_has_no_permission()
    {
        // Arrange
        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.UpdateCampus(1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    #endregion

    #region Validation errors

    [Test]
    [TestCase("")]
    public async Task Campi_UpdateCampus_Should_not_update_campus_with_invalid_name(string name)
    {
        // Arrange
        var client = await _back.LoggedAsDirectot();
        var campus = await client.CreateCampus();

        // Act
        var response = await client.UpdateCampus(campus.Success.Id, name, BrazilState.PE, "Bonito", 200);

        // Assert
        response.ShouldBeError(InvalidCampusName.I);
    }

    [Test]
    [TestCase(null)]
    [TestCase((BrazilState)69)]
    public async Task Campi_UpdateCampus_Should_not_update_campus_with_invalid_brazil_state(BrazilState? state)
    {
        // Arrange
        var client = await _back.LoggedAsDirectot();
        var campus = await client.CreateCampus();

        // Act
        var response = await client.UpdateCampus(campus.Success.Id, "Agreste II", state, "Bonito", 200);

        // Assert
        response.ShouldBeError(InvalidBrazilState.I);
    }

    [Test]
    [TestCase("")]
    public async Task Campi_UpdateCampus_Should_not_update_campus_with_invalid_city(string city)
    {
        // Arrange
        var client = await _back.LoggedAsDirectot();
        var campus = await client.CreateCampus();

        // Act
        var response = await client.UpdateCampus(campus.Success.Id, "Agreste II", BrazilState.PE, city, 200);

        // Assert
        response.ShouldBeError(InvalidCampusCity.I);
    }

    [Test]
    [TestCase(0)]
    [TestCase(-1)]
    public async Task Campi_UpdateCampus_Should_not_update_campus_with_invalid_capacity(int capacity)
    {
        // Arrange
        var client = await _back.LoggedAsDirectot();
        var campus = await client.CreateCampus();

        // Act
        var response = await client.UpdateCampus(campus.Success.Id, "Agreste II", BrazilState.PE, "Bonito", capacity);

        // Assert
        response.ShouldBeError(InvalidCampusCapacity.I);
    }

    [Test]
    public async Task Campi_UpdateCampus_Should_not_update_campus_not_found()
    {
        // Arrange
        var client = await _back.LoggedAsDirectot();

        // Act
        var response = await client.UpdateCampus(99999);

        // Assert
        response.ShouldBeError(CampusNotFound.I);
    }

    [Test]
    public async Task Campi_UpdateCampus_Should_not_update_other_institution_campus()
    {
        // Arrange
        var client = await _back.LoggedAsDirectot();

        var otherClient = await _back.LoggedAsDirectot();
        var otherCampus = await otherClient.CreateCampus();

        // Act
        var response = await client.UpdateCampus(otherCampus.Success.Id);

        // Assert
        response.ShouldBeError(CampusNotFound.I);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task Campi_UpdateCampus_Should_update_campus()
    {
        // Arrange
        var client = await _back.LoggedAsDirectot();
        var campus = await client.CreateCampus();

        // Act
        var result = await client.UpdateCampus(campus.Success.Id, "Agreste II", BrazilState.PE, "Bonito", 789);

        // Assert
        var updated = result.Success;
        updated.Id.Should().Be(campus.Success.Id);
        updated.Name.Should().Be("Agreste II");
        updated.State.Should().Be(BrazilState.PE);
        updated.City.Should().Be("Bonito");
        updated.Capacity.Should().Be(789);
    }

    #endregion
}
