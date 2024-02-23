namespace Syki.Tests.Auth;

public partial class AuthTests : AuthTestBase
{
    [Test]
    public async Task Should_return_unauthorized_when_unauthenticated_user_wants_to_setup_mfa_key()
    {
        // Arrange
        Logout();

        // Act
        var response = await _client.PostAsync("/mfa/setup", null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
