namespace Estud.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Home_Should_redirect_to_api_docs_page()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var response = await client.GetHome();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
