namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Should_redirect_to_api_docs_page()
    {
        // Arrange
        var client = _factory.GetClient();

        // Act
        var response = await client.GetAsync("/");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.RequestMessage!.RequestUri!.ToString().Should().Contain("/docs");
    }
}
