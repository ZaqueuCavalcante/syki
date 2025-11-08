namespace Exato.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Should_reset_password()
    {
        // Arrange
        var user = (await _back.LoggedAsOfficeCS()).User;
        var client = _back.GetClient();

        await client.SendResetPasswordToken(user.Email);
        var token = await _back.GetResetPasswordToken(user.Email);

        // Act
        var password = "My@new@strong@P4ssword";
        var response = await client.ResetPassword(token!, password);
        var login = await client.Login(user.Email, password);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var loginOut = login.Success;
        loginOut.Id.Should().Be(user.Id);
        loginOut.Name.Should().Be(user.Name);
        loginOut.Email.Should().Be(user.Email);
        loginOut.Role.Should().Be("OfficeCustomerSuccess");
    }

    [Test]
    public async Task Should_not_reset_password_with_wrong_token()
    {
        // Arrange
        var user = (await _back.LoggedAsOfficeCS()).User;
        var client = _back.GetClient();

        await client.SendResetPasswordToken(user.Email);

        // Act
        var password = "My@new@strong@P4ssword";
        var response = await client.ResetPassword("token_errado_lalala", password);

        // Assert
        await response.AssertBadRequest(UserNotFound.I);
    }

    [Test]
    public async Task Should_not_login_using_the_old_password()
    {
        // Arrange
        var user = (await _back.LoggedAsOfficeCS()).User;
        var client = _back.GetClient();

        await client.SendResetPasswordToken(user.Email);
        var token = await _back.GetResetPasswordToken(user.Email);

        var password = "My@new@strong@P4ssword";
        await client.ResetPassword(token!, password);

        // Act
        var response = await client.Login(user.Email, ExatoCSPassword);

        // Assert
        response.ShouldBeError(LoginWrongEmailOrPassword.I);
    }

    [Test]
    public async Task Should_not_reset_password_twice_with_same_token()
    {
        // Arrange
        var user = (await _back.LoggedAsOfficeCS()).User;
        var client = _back.GetClient();

        await client.SendResetPasswordToken(user.Email);
        var token = await _back.GetResetPasswordToken(user.Email);

        var password = "My@new@strong@P4ssword";
        await client.ResetPassword(token!, password);

        // Act
        var response = await client.ResetPassword(token!, password);

        // Assert
        await response.AssertBadRequest(InvalidResetPasswordToken.I);
    }

    [Test]
    [TestCaseSource(nameof(InvalidPasswords))]
    public async Task Should_not_reset_password_to_a_weak_one(string password)
    {
        // Arrange
        var user = (await _back.LoggedAsOfficeCS()).User;
        var client = _back.GetClient();

        await client.SendResetPasswordToken(user.Email);
        var token = await _back.GetResetPasswordToken(user.Email);

        // Act
        var response = await client.ResetPassword(token!, password);

        // Assert
        await response.AssertBadRequest(WeakPassword.I);
    }

    private static IEnumerable<object[]> InvalidPasswords()
    {
        foreach (var role in new List<string>()
        {
            "",
            " ",
            "capi",
            "capi123",
            "Capi123",
            "lalal.com",
            "12@3lalala",
            "5816811681816",
        })
        {
            yield return [role];
        }
    }
}
