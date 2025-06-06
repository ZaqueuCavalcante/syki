using Syki.Back.Features.Cross.SendResetPasswordToken;

namespace Syki.Tests.Features.Cross.SendResetPasswordToken;

public class SendResetPasswordTokenUnitTests
{
    [Test]
    public void Should_create_a_reset_password_with_correct_data()
    {
        // Arrange
        var userId = Guid.CreateVersion7();
        var institutionId = Guid.CreateVersion7();
        var token = Guid.CreateVersion7().ToString();

        // Act
        var reset = new ResetPasswordToken(userId, institutionId, token);

        // Assert
        reset.Id.Should().NotBeEmpty();
        reset.UserId.Should().Be(userId);
        reset.Token.Should().Be(token);
        reset.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        reset.UsedAt.Should().BeNull();
    }
}
