namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    // [Test]
    public async Task Should_login()
    {
        // Arrange
        var client = _back.GetClient();
        var user = await client.RegisterAcademicUser(_back);

        // Act
        var result = await client.Login(user.Email, user.Password);

        // Assert
        var loginOut = result.Success;
        loginOut.Id.Should().Be(user.Id);
        loginOut.Name.Should().Be(user.Email);
        loginOut.Email.Should().Be(user.Email);
        loginOut.Role.Should().Be(UserRole.Academic);
    }

    // [Test]
    public async Task Should_not_login_random_user()
    {
        // Arrange
        var client = _back.GetClient();
        await client.RegisterAcademicUser(_back);
        var email = "academico@novaroma.com";
        var password = "Academico@123";

        // Act
        var result = await client.Login(email, password);

        // Assert
        result.ShouldBeError(new LoginWrongEmailOrPassword());
    }

    // [Test]
    public async Task Should_not_login_user_with_wrong_email()
    {
        // Arrange
        var client = _back.GetClient();
        var user = await client.RegisterAcademicUser(_back);

        // Act
        var result = await client.Login(user.Email + "1", user.Password);

        // Assert
        result.ShouldBeError(new LoginWrongEmailOrPassword());
    }

    // [Test]
    public async Task Should_not_login_user_with_wrong_password()
    {
        // Arrange
        var client = _back.GetClient();
        var user = await client.RegisterAcademicUser(_back);

        // Act
        var result = await client.Login(user.Email, user.Password + "1");

        // Assert
        result.ShouldBeError(new LoginWrongEmailOrPassword());
    }

    // [Test]
    public async Task Should_not_login_user_with_correct_email_and_password_but_needs_mfa()
    {
        // Arrange
        var client = _back.GetClient();
        var user = await client.RegisterAcademicUser(_back);
        await client.Login(user.Email, user.Password);

        var keyResponse = await client.GetMfaKey();
        var token = keyResponse.Key.GenerateTOTP();
        await client.SetupMfa(token);

        await client.Logout();

        // Act
        var result = await client.Login(user.Email, user.Password);

        // Assert
        result.ShouldBeError(new LoginRequiresTwoFactor());
    }
}
