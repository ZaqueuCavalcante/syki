using Syki.Back.Features.Cross.SendResetPasswordToken;

namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_send_the_reset_password_token()
    {
        // Arrange
        var client = _api.GetClient();
        var user = await client.RegisterUser(_api);

        // Act
        await client.SendResetPasswordToken(user.Email);

        // Assert
        var token = await _api.GetResetPasswordToken(user.Email);
        token!.Length.Should().Be(36);
    }

    [Test]
    public async Task Should_enqueue_reset_password_token_task()
    {
        // Arrange
        var client = _api.GetClient();
        var user = await client.RegisterUser(_api);

        // Act
        await client.SendResetPasswordToken(user.Email);

        // Assert
        using var ctx = _api.GetDbContext();
        var userDb = await ctx.Users.FirstAsync(x => x.Email == user.Email);
        await AssertTaskByDataLike<SendResetPasswordEmail>(userDb.Id.ToString());
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
