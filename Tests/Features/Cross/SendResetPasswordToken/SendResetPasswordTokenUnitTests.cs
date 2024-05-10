using Syki.Back.Features.Cross.SendResetPasswordToken;

namespace Syki.Tests.Features.Cross.SendResetPasswordToken;

public class SendResetPasswordTokenUnitTests
{
    [Test]
    public void Should_create_a_reset_password_with_correct_id()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var token = Guid.NewGuid().ToString();

        // Act
        var reset = new ResetPasswordToken(userId, token);

        // Assert
        reset.Id.Should().NotBeEmpty();
    }

    [Test]
    public void Should_create_a_reset_password_with_correct_user_id()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var token = Guid.NewGuid().ToString();

        // Act
        var reset = new ResetPasswordToken(userId, token);

        // Assert
        reset.UserId.Should().Be(userId);
    }

    [Test]
    public void Should_create_a_reset_password_with_correct_token()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var token = Guid.NewGuid().ToString();

        // Act
        var reset = new ResetPasswordToken(userId, token);

        // Assert
        reset.Token.Should().Be(token);
    }

    [Test]
    public void Should_create_a_reset_password_with_correct_created_at()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var token = Guid.NewGuid().ToString();

        // Act
        var reset = new ResetPasswordToken(userId, token);

        // Assert
        reset.CreatedAt.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
    }

    [Test]
    public void Should_create_a_reset_password_with_null_used_at()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var token = Guid.NewGuid().ToString();

        // Act
        var reset = new ResetPasswordToken(userId, token);

        // Assert
        reset.UsedAt.Should().BeNull();
    }
}
