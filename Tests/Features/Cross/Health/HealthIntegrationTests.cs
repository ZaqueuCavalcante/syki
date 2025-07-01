namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_get_api_healthy_status()
    {
        // Arrange
        var client = _api.GetClient();

        // Act
        var response = await client.GetAsync("/health");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Test]
    public async Task Should_get_daemon_healthy_status()
    {
        // Arrange
        var client = _daemon.GetClient();

        // Act
        var response = await client.GetAsync("/health");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
