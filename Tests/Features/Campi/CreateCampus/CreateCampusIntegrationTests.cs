namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    #region Authentication

    [Test]
    public async Task Campi_CreateCampus_Should_not_create_campus_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.CreateCampus();

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Authorization

    [Test]
    public async Task Campi_CreateCampus_Should_not_create_campus_when_user_has_no_permission()
    {
        // Arrange
        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.CreateCampus();

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    #endregion

    #region Validation errors

    [Test]
    [TestCase("")]
    public async Task Campi_CreateCampus_Should_not_create_campus_with_invalid_name(string name)
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

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
        var client = await _back.LoggedAsDirector();

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
        var client = await _back.LoggedAsDirector();

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
        var client = await _back.LoggedAsDirector();

        // Act
        var response = await client.CreateCampus("Agreste", BrazilState.PE, "Caruaru", capacity);

        // Assert
        response.ShouldBeError(InvalidCampusCapacity.I);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task Campi_CreateCampus_Should_create_campus()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.CreateCampus("Agreste I", BrazilState.PE, "Caruaru", 123);

        // Assert
        var campus = result.Success;
        campus.Id.Should().NotBe(0);
    }

    #endregion
}
