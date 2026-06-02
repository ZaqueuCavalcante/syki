namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Cross_Home_Should_redirect_to_docs()
    {
        // Arrange
        var client = _back.GetNoRedirectTestsClient();

        // Act
        var response = await client.GetHome();

        // Assert
        ((int)response.StatusCode).Should().BeInRange(300, 399);
    }
}
