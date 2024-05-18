namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Should_return_all_users()
    {
        // Arrange
        var client = _factory.GetClient();
        await client.RegisterUser(_factory);

        var admClient = await _factory.LoggedAsAdm();

        // Act
        var users = await admClient.GetUsers();

        // Assert
        users.Should().HaveCountGreaterThanOrEqualTo(2);
    }
}
