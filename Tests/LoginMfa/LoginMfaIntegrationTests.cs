namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Should_not_login_when_try_get_jwt_with_right_mfa_code_but_without_supply_email_and_password()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademico();

        var keyResponse = await client.GetMfaKey();
        var token = keyResponse.Key.ToMfaToken();
        await client.SetupMfa(token);

        client.RemoveAuthToken();

        // Act
        var response = await client.LoginMfa(token);

        // Assert
        response.AccessToken.Should().BeNull();
        response.Wrong2FactorCode.Should().BeTrue();
    }

    [Test]
    public async Task Should_not_login_when_supply_wrong_mfa_code()
    {
        // Arrange
        var client = _factory.GetClient();
        var user = await client.RegisterUser(_factory);
        await client.Login(user.Email, user.Password);

        var keyResponse = await client.GetMfaKey();
        var token = keyResponse.Key.ToMfaToken();
        await client.SetupMfa(token);

        client.RemoveAuthToken();

        await client.Login(user.Email, user.Password);

        // Act
        var response = await client.LoginMfa(Guid.NewGuid().ToHashCode().ToString()[..6]);

        // Assert
        response.AccessToken.Should().BeNull();
        response.Wrong2FactorCode.Should().BeTrue();
    }

    [Test]
    public async Task Should_login_when_supply_right_mfa_code()
    {
        // Arrange
        var client = _factory.GetClient();
        var user = await client.RegisterUser(_factory);
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
}
