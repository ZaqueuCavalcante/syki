using Syki.Back.CreateUser;

namespace Syki.Tests.CreateUser;

public class CreateUserUnitTests
{
    [Test]
    public void Should_create_a_user_with_right_name()
    {
        // Arrange
        var institutionId = Guid.NewGuid();
        const string name = "Zaqueu";
        const string email = "zaqueu@syki.com";

        // Act
        var user = new SykiUser(institutionId, name, email);

        // Assert
        user.Name.Should().Be(name);
    }

    [Test]
    public void Should_create_a_user_with_right_user_name()
    {
        // Arrange
        var institutionId = Guid.NewGuid();
        const string name = "Zaqueu";
        const string email = "zaqueu@syki.com";

        // Act
        var user = new SykiUser(institutionId, name, email);

        // Assert
        user.UserName.Should().Be(email);
    }

    [Test]
    public void Should_create_a_user_with_right_email()
    {
        // Arrange
        var institutionId = Guid.NewGuid();
        const string name = "Zaqueu";
        const string email = "zaqueu@syki.com";

        // Act
        var user = new SykiUser(institutionId, name, email);

        // Assert
        user.Email.Should().Be(email);
    }

    [Test]
    public void Should_create_a_user_with_right_created_at()
    {
        // Arrange
        var institutionId = Guid.NewGuid();
        const string name = "Zaqueu";
        const string email = "zaqueu@syki.com";

        // Act
        var user = new SykiUser(institutionId, name, email);

        // Assert
        user.CreatedAt.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
    }
}
