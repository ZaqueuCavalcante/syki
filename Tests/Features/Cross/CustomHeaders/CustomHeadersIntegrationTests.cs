namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_get_api_custom_headers()
    {
        // Arrange
        var client = _api.GetClient();

        // Act
        var response = await client.GetAsync("/health");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Headers.GetValues("X-DeployHash").Should().BeEquivalentTo([Env.DeployHash]);
        response.Headers.GetValues("X-CrossLogin").Should().BeEquivalentTo(["True"]);
    }
}
