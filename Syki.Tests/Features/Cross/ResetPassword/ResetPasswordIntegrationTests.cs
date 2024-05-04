namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Should_get_the_reset_password_token()
    {
        // Arrange
        var client = _factory.GetClient();
        var user = await client.RegisterUser(_factory);
        await client.SendResetPasswordToken(user.Email);

        // Act
        var token = await _factory.GetResetPasswordToken(user.Email);

        // Assert
        token!.Length.Should().Be(36);
    }

    [Test]
    public async Task Should_not_get_the_reset_password_token_when_user_not_exists()
    {
        // Arrange / Act
        var token = await _factory.GetResetPasswordToken(TestData.Email);

        // Assert
        token.Should().BeNull();
    }

    [Test]
    public async Task Should_not_reset_password_with_wrong_token()
    {
        // Arrange
        var client = _factory.GetClient();
        var user = await client.RegisterUser(_factory);
        await client.SendResetPasswordToken(user.Email);

        client.RemoveAuthToken();

        // Act
        var response = await client.ResetPassword(Guid.NewGuid().ToString(), "My@new@strong@P4ssword");

        // Assert
        await response.AssertBadRequest(Throw.DE019);
    }

    [Test]
    public async Task Should_reset_password()
    {
        // Arrange
        var client = _factory.GetClient();
        var user = await client.RegisterUser(_factory);
        await client.SendResetPasswordToken(user.Email);

        client.RemoveAuthToken();
        var token = await _factory.GetResetPasswordToken(user.Email);

        // Act
        var response = await client.ResetPassword(token!, "My@new@strong@P4ssword");

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
    }

    [Test]
    public async Task Should_login_using_the_new_password()
    {
        // Arrange
        var client = _factory.GetClient();
        var user = await client.RegisterUser(_factory);
        await client.SendResetPasswordToken(user.Email);

        client.RemoveAuthToken();
        var token = await _factory.GetResetPasswordToken(user.Email);
        var password = "My@new@strong@P4ssword";
        await client.ResetPassword(token!, password);

        // Act
        var response = await client.Login(user.Email, password);

        // Assert
        response.AccessToken.Should().StartWith("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.");
    }

    [Test]
    public async Task Should_not_login_using_the_old_password()
    {
        // Arrange
        var client = _factory.GetClient();
        var user = await client.RegisterUser(_factory);
        await client.SendResetPasswordToken(user.Email);

        client.RemoveAuthToken();
        var token = await _factory.GetResetPasswordToken(user.Email);
        var password = "My@new@strong@P4ssword";
        await client.ResetPassword(token!, password);

        // Act
        var response = await client.Login(user.Email, user.Password);

        // Assert
        response.WrongEmailOrPassword.Should().BeTrue();
    }

    [Test]
    public async Task Should_not_reset_password_twice_with_same_token()
    {
        // Arrange
        var client = _factory.GetClient();
        var user = await client.RegisterUser(_factory);
        await client.SendResetPasswordToken(user.Email);

        client.RemoveAuthToken();
        var token = await _factory.GetResetPasswordToken(user.Email);
        var password = "My@new@strong@P4ssword";
        await client.ResetPassword(token!, password);

        // Act
        var response = await client.ResetPassword(token!, password);

        // Assert
        await response.AssertBadRequest(Throw.DE020);
    }

    [Test]
    [TestCaseSource(typeof(TestData), nameof(TestData.InvalidPasswords))]
    public async Task Should_not_reset_password_to_a_weak_one(string password)
    {
        // Arrange
        var client = _factory.GetClient();
        var user = await client.RegisterUser(_factory);
        await client.SendResetPasswordToken(user.Email);

        client.RemoveAuthToken();
        var token = await _factory.GetResetPasswordToken(user.Email);

        // Act
        var response = await client.ResetPassword(token!, password);

        // Assert
        await response.AssertBadRequest(Throw.DE015);
    }
}
