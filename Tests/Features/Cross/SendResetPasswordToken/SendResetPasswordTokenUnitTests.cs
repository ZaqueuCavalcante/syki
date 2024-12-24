using Syki.Back.Features.Cross.SendResetPasswordToken;

namespace Syki.Tests.Features.Cross.SendResetPasswordToken;

public class SendResetPasswordTokenUnitTests
{
    [Test]
    public void Should_create_a_reset_password_with_correct_data()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var institutionId = Guid.NewGuid();
        var token = Guid.NewGuid().ToString();

        // Act
        var reset = new ResetPasswordToken(userId, institutionId, token);

        // Assert
        reset.Id.Should().NotBeEmpty();
        reset.UserId.Should().Be(userId);
        reset.Token.Should().Be(token);
        reset.CreatedAt.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
        reset.UsedAt.Should().BeNull();
    }
}
