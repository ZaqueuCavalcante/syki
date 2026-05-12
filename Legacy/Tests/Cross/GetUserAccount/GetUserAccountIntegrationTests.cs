namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    // [Test]
    public async Task Should_get_user_account()
    {
        // Arrange
        var client = _back.GetClient();
        var user = await client.RegisterAcademicUser(_back);
        await client.Login(user.Email, user.Password);

        // Act
        var response = await client.GetUserAccount();

        // Assert
        response.Id.Should().Be(user.Id);
        response.Name.Should().Be(user.Email);
        response.Email.Should().Be(user.Email);
        response.Role.Should().Be(UserRole.Academic);
    }
}
