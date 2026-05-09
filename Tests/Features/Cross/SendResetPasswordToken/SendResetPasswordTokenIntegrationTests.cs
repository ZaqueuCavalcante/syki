using Syki.Back.Emails;
using Syki.Back.Features.Cross.SendResetPasswordToken;

namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    // [Test]
    public async Task Should_send_reset_password_token()
    {
        // Arrange
        var client = _back.GetClient();
        var user = await client.RegisterAcademicUser(_back);

        // Act
        await client.SendResetPasswordToken(user.Email);

        // Assert
        var token = await _back.GetResetPasswordToken(user.Email);
        token!.Length.Should().Be(36);
    }

    // [Test]
    public async Task Should_send_a_reset_password_email()
    {
        // Arrange
        var client = _back.GetClient();
        var user = await client.RegisterAcademicUser(_back);

        // Act
        await client.SendResetPasswordToken(user.Email);

        // Assert
        // await _api.AwaitEventsProcessing();
        // await _api.AwaitCommandsProcessing();

        // var service = _api.GetService<IEmailsService>() as FakeEmailsService;
        // service!.ResetPasswordEmails.Should().ContainSingle(x => x.Contains(user.Email));
    }

    // [Test]
    public async Task Should_not_send_a_reset_password_email_when_user_not_exists()
    {
        // Arrange
        var client = _back.GetClient();
        var email = TestData.Email;

        // Act
        var response = await client.SendResetPasswordToken(email);

        // Assert
        await response.AssertBadRequest(new UserNotFound());
    }

    // [Test]
    public async Task Should_not_get_the_reset_password_token_when_user_not_exists()
    {
        // Arrange / Act
        var token = await _back.GetResetPasswordToken(TestData.Email);

        // Assert
        token.Should().BeNull();
    }
}
