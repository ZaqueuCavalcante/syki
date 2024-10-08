namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_login_when_supply_right_mfa_token()
    {
        // Arrange
        var client = _back.GetClient();
        var user = await client.RegisterUser(_back);
        await client.Login(user.Email, user.Password);

        var keyResponse = await client.GetMfaKey();
        var token = keyResponse.Key.ToMfaToken();
        await client.SetupMfa(token);

        client.RemoveAuthToken();

        await client.Login(user.Email, user.Password);

        // Act
        var response = await client.LoginMfa(token);

        // Assert
        response.AccessToken.Should().StartWith("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.");
    }

    [Test]
    public async Task Should_not_login_when_try_get_jwt_with_right_mfa_token_but_without_supply_email_and_password()
    {
        // Arrange
        var client = await _back.LoggedAsAcademic();

        var keyResponse = await client.Http.GetMfaKey();
        var token = keyResponse.Key.ToMfaToken();
        await client.Http.SetupMfa(token);

        client.Http.RemoveAuthToken();

        // Act
        var response = await client.Http.LoginMfa(token);

        // Assert
        response.AccessToken.Should().BeNull();
        response.Wrong2FactorCode.Should().BeTrue();
    }

    [Test]
    public async Task Should_not_login_when_supply_wrong_mfa_token()
    {
        // Arrange
        var client = _back.GetClient();
        var user = await client.RegisterUser(_back);
        await client.Login(user.Email, user.Password);

        var keyResponse = await client.GetMfaKey();
        var token = keyResponse.Key.ToMfaToken();
        await client.SetupMfa(token);

        client.RemoveAuthToken();

        await client.Login(user.Email, user.Password);
        var randomToken = Guid.NewGuid().ToHashCode().ToString()[..6];

        // Act
        var response = await client.LoginMfa(randomToken);

        // Assert
        response.AccessToken.Should().BeNull();
        response.Wrong2FactorCode.Should().BeTrue();
    }
}
