namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    #region Authentication

    [Test]
    public async Task Campi_GetCampi_Should_not_get_campi_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.GetCampi();

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Authorization

    [Test]
    public async Task Campi_GetCampi_Should_not_get_campi_when_user_has_no_permission()
    {
        // Arrange
        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.GetCampi();

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task Campi_GetCampi_Should_get_campi()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        await client.CreateCampus("Suassuna", BrazilState.PE, "Recife", 150);
        await client.CreateCampus("Agreste", BrazilState.PE, "Caruaru", 120);

        // Act
        var result = await client.GetCampi();

        // Assert
        result.Success.Total.Should().Be(2);
        result.Success.Items.First().Name.Should().Be("Agreste");
        result.Success.Items.Last().Name.Should().Be("Suassuna");
    }

    #endregion
}
