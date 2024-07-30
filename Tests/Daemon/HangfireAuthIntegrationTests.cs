namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_return_401_on_get_hangfire_dashboard_without_auth_data()
    {
        // Arrange
        var client = _daemon.CreateClient();

        // Act
        var response = await client.GetAsync("/");

        // Assert
        response.Should().HaveStatusCode(HttpStatusCode.Unauthorized);
    }

    [Test]
    public async Task Should_return_401_on_get_hangfire_dashboard_with_wrong_auth_data()
    {
        // Arrange
        var client = _daemon.CreateClient();

        var data = "user:password".ToBase64();
        client.DefaultRequestHeaders.Add("Authorization", $"Basic {data}");
        
        // Act
        var response = await client.GetAsync("/");

        // Assert
        response.Should().HaveStatusCode(HttpStatusCode.Unauthorized);
    }
    
    [Test]
    public async Task Should_return_200_on_get_hangfire_dashboard_with_correct_auth_data()
    {
        // Arrange
        var client = _daemon.CreateClient();

        var data = "User:Password".ToBase64();
        client.DefaultRequestHeaders.Add("Authorization", $"Basic {data}");
        
        // Act
        var response = await client.GetAsync("/");

        // Assert
        response.Should().HaveStatusCode(HttpStatusCode.OK);
    }
}
