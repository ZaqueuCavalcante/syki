namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_return_all_users()
    {
        // Arrange
        await _api.LoggedAsAcademic();

        var client = await _api.LoggedAsAdm();

        // Act
        var users = await client.GetUsers();

        // Assert
        users.Should().HaveCountGreaterThanOrEqualTo(2);
    }
}
