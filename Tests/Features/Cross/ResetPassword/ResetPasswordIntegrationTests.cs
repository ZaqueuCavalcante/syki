namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_reset_password()
    {
        // Arrange
        var client = _api.GetClient();
        var user = await client.RegisterUser(_api);
        await client.SendResetPasswordToken(user.Email);

        client.Logout();
        var token = await _api.GetResetPasswordToken(user.Email);
        var password = "My@new@strong@P4ssword";

        // Act
        var response = await client.ResetPassword(token!, password);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var login = await client.Login(user.Email, password);
        login.GetSuccess().AccessToken.Should().StartWith("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.");
    }

    [Test]
    public async Task Should_not_reset_password_with_wrong_token()
    {
        // Arrange
        var client = _api.GetClient();
        var user = await client.RegisterUser(_api);
        await client.SendResetPasswordToken(user.Email);

        client.Logout();

        // Act
        var response = await client.ResetPassword(Guid.NewGuid().ToString(), "My@new@strong@P4ssword");

        // Assert
        await response.AssertBadRequest(new UserNotFound());
    }

    [Test]
    public async Task Should_not_login_using_the_old_password()
    {
        // Arrange
        var client = _api.GetClient();
        var user = await client.RegisterUser(_api);
        await client.SendResetPasswordToken(user.Email);

        client.Logout();
        var token = await _api.GetResetPasswordToken(user.Email);
        var password = "My@new@strong@P4ssword";
        await client.ResetPassword(token!, password);

        // Act
        var response = await client.Login(user.Email, user.Password);

        // Assert
        response.ShouldBeError(new LoginWrongEmailOrPassword());
    }

    [Test]
    public async Task Should_not_reset_password_twice_with_same_token()
    {
        // Arrange
        var client = _api.GetClient();
        var user = await client.RegisterUser(_api);
        await client.SendResetPasswordToken(user.Email);

        client.Logout();
        var token = await _api.GetResetPasswordToken(user.Email);
        var password = "My@new@strong@P4ssword";
        await client.ResetPassword(token!, password);

        // Act
        var response = await client.ResetPassword(token!, password);

        // Assert
        await response.AssertBadRequest(new InvalidResetToken());
    }

    [Test]
    [TestCaseSource(typeof(TestData), nameof(TestData.InvalidPasswords))]
    public async Task Should_not_reset_password_to_a_weak_one(string password)
    {
        // Arrange
        var client = _api.GetClient();
        var user = await client.RegisterUser(_api);
        await client.SendResetPasswordToken(user.Email);

        client.Logout();
        var token = await _api.GetResetPasswordToken(user.Email);

        // Act
        var response = await client.ResetPassword(token!, password);

        // Assert
        await response.AssertBadRequest(new WeakPassword());
    }
}
