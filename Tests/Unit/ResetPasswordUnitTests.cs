using Syki.Back.SendResetPasswordToken;

namespace Syki.Tests.Unit;

public class ResetPasswordUnitTests
{
    [Test]
    public void Deve_criar_um_reset_password_com_id_correto()
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
    public void Deve_criar_um_reset_password_com_user_id_correto()
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
    public void Deve_criar_um_reset_password_com_token_correto()
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
    public void Deve_criar_um_reset_password_com_created_at_correta()
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
    public void Deve_criar_um_reset_password_com_used_at_nula()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var token = Guid.NewGuid().ToString();

        // Act
        var reset = new ResetPasswordToken(userId, token);

        // Assert
        reset.UsedAt.Should().BeNull();
    }

    [Test]
    public void Deve_usar_um_reset_password()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var token = Guid.NewGuid().ToString();
        var reset = new ResetPasswordToken(userId, token);

        // Act
        reset.Use();

        // Assert
        reset.UsedAt.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
    }
}
