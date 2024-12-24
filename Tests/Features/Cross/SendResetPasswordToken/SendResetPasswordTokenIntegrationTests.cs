using Syki.Back.Emails;
using Syki.Back.Features.Cross.SendResetPasswordToken;

namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_send_reset_password_token()
    {
        // Arrange
        var client = _api.GetClient();
        var user = await client.RegisterUser(_api);

        // Act
        await client.SendResetPasswordToken(user.Email);

        // Assert
        var token = await _api.GetResetPasswordToken(user.Email);
        token!.Length.Should().Be(36);

        await AssertDomainEvent<ResetPasswordTokenCreatedDomainEvent>(user.Id.ToString());
    }

    [Test]
    public async Task Should_send_a_reset_password_email()
    {
        // Arrange
        var client = _api.GetClient();
        var user = await client.RegisterUser(_api);

        // Act
        await client.SendResetPasswordToken(user.Email);

        // Assert
        await _daemon.AwaitEventsProcessing();
        await _daemon.AwaitTasksProcessing();

        var service = _daemon.GetService<IEmailsService>() as FakeEmailsService;
        service!.ResetPasswordEmails.Should().ContainSingle(x => x.Contains(user.Email));
    }

    [Test]
    public async Task Should_not_send_a_reset_password_email_when_user_not_exists()
    {
        // Arrange
        var client = _api.GetClient();
        var email = TestData.Email;

        // Act
        var response = await client.SendResetPasswordToken(email);

        // Assert
        await response.AssertBadRequest(new UserNotFound());
    }

    [Test]
    public async Task Should_not_get_the_reset_password_token_when_user_not_exists()
    {
        // Arrange / Act
        var token = await _api.GetResetPasswordToken(TestData.Email);

        // Assert
        token.Should().BeNull();
    }
}
