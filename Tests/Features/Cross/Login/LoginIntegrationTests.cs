namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Should_login()
    {
        // Arrange
        var client = _back.GetClient();
        var user = await client.RegisterUser(_back);

        // Act
        var result = await client.Login(user.Email, user.Password);

        // Assert
        result.AccessToken.Should().StartWith("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.");
    }

    [Test]
    public async Task Should_not_login_random_user()
    {
        // Arrange
        var client = _back.GetClient();
        await client.RegisterUser(_back);
        var email = "academico@novaroma.com";
        var password = "Academico@123";

        // Act
        var result = await client.Login(email, password);

        // Assert
        result.WrongEmailOrPassword.Should().BeTrue();
    }

    [Test]
    public async Task Should_not_login_user_with_wrong_email()
    {
        // Arrange
        var client = _back.GetClient();
        var user = await client.RegisterUser(_back);

        // Act
        var result = await client.Login(user.Email + "1", user.Password);

        // Assert
        result.WrongEmailOrPassword.Should().BeTrue();
    }

    [Test]
    public async Task Should_not_login_user_with_wrong_password()
    {
        // Arrange
        var client = _back.GetClient();
        var user = await client.RegisterUser(_back);

        // Act
        var result = await client.Login(user.Email, user.Password + "1");

        // Assert
        result.WrongEmailOrPassword.Should().BeTrue();
    }

    [Test]
    public async Task Should_not_login_user_with_correct_email_and_password_but_needs_mfa()
    {
        // Arrange
        var client = _back.GetClient();
        var user = await client.RegisterUser(_back);
        await client.Login(user.Email, user.Password);

        var keyResponse = await client.GetMfaKey();
        var token = keyResponse.Key.ToMfaToken();
        await client.SetupMfa(token);

        client.RemoveAuthToken();

        // Act
        var result = await client.Login(user.Email, user.Password);

        // Assert
        result.RequiresTwoFactor.Should().BeTrue();
    }
}
