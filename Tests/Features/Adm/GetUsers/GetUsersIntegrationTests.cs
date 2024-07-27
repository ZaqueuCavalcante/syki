namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_return_all_users()
    {
        // Arrange
        await _back.LoggedAsAcademic();

        var client = await _back.LoggedAsAdm();

        // Act
        var users = await client.GetUsers();

        // Assert
        users.Should().HaveCountGreaterThanOrEqualTo(2);
    }
}
