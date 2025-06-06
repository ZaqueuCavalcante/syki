using Syki.Back.Features.Cross.SendResetPasswordToken;

namespace Syki.Tests.Features.Cross.ResetPassword;

public class ResetPasswordUnitTests
{
    [Test]
    public void Should_use_a_reset_password_token()
    {
        // Arrange
        var userId = Guid.CreateVersion7();
        var institutionId = Guid.CreateVersion7();
        var token = Guid.CreateVersion7().ToString();
        var reset = new ResetPasswordToken(userId, institutionId, token);

        // Act
        reset.Use();

        // Assert
        reset.UsedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }
}
