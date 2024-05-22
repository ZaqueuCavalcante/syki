using Syki.Back.Features.Cross.SendResetPasswordToken;

namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Should_send_the_reset_password_token()
    {
        // Arrange
        var client = _back.GetClient();
        var user = await client.RegisterUser(_back);

        // Act
        await client.SendResetPasswordToken(user.Email);

        // Assert
        var token = await _back.GetResetPasswordToken(user.Email);
        token!.Length.Should().Be(36);
    }

    [Test]
    public async Task Should_enqueue_reset_password_token_task()
    {
        // Arrange
        var client = _back.GetClient();
        var user = await client.RegisterUser(_back);

        // Act
        await client.SendResetPasswordToken(user.Email);

        // Assert
        using var ctx = _back.GetDbContext();
        var userDb = await ctx.Users.FirstAsync(x => x.Email == user.Email);
        await AssertTaskByDataLike<SendResetPasswordEmail>(userDb.Id.ToString());
    }

    [Test]
    public async Task Should_not_get_the_reset_password_token_when_user_not_exists()
    {
        // Arrange / Act
        var token = await _back.GetResetPasswordToken(TestData.Email);

        // Assert
        token.Should().BeNull();
    }
}
