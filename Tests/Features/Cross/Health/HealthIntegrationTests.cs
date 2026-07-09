namespace Estud.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Cross_Health_Should_return_healthy()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var response = await client.GetHealth();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
