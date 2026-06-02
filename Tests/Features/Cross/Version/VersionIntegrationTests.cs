namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Cross_Version_Should_return_version()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var response = await client.GetVersion();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
