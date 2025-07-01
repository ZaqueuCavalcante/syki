namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_login_when_supply_right_totp()
    {
        // Arrange
        var client = _api.GetClient();
        var user = await client.RegisterUser(_api);
        await client.Login(user.Email, user.Password);

        var keyResponse = await client.GetMfaKey();
        var totp = keyResponse.Key.GenerateTOTP();
        await client.SetupMfa(totp);

        client.Logout();
        await client.Login(user.Email, user.Password);

        // Act
        var response = await client.LoginMfa(totp);

        // Assert
        var claims = response.GetSuccess().AccessToken.ToClaims();
        claims.First(x => x.Type == "email").Value.Should().Be(user.Email);
        claims.First(x => x.Type == "sub").Value.Should().Be(user.Id.ToString());
        claims.First(x => x.Type == "role").Value.Should().Be(UserRole.Academic.ToString());
    }

    [Test]
    public async Task Should_not_login_with_right_totp_but_without_supply_email_and_password()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();

        var keyResponse = await client.Http.GetMfaKey();
        var totp = keyResponse.Key.GenerateTOTP();
        await client.Http.SetupMfa(totp);

        // Act
        var response = await client.Http.LoginMfa(totp);

        // Assert
        response.ShouldBeError(new LoginWrongMfaToken());
    }

    [Test]
    public async Task Should_not_login_when_supply_wrong_totp()
    {
        // Arrange
        var client = _api.GetClient();
        var user = await client.RegisterUser(_api);
        await client.Login(user.Email, user.Password);

        var keyResponse = await client.GetMfaKey();
        var totp = keyResponse.Key.GenerateTOTP();
        await client.SetupMfa(totp);

        await client.Login(user.Email, user.Password);
        var randomToken = Guid.NewGuid().ToHashCode().ToString()[..6];

        // Act
        var response = await client.LoginMfa(randomToken);

        // Assert
        response.ShouldBeError(new LoginWrongMfaToken());
    }
}
