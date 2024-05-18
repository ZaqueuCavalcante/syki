namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Should_return_all_users()
    {
        // Arrange
        await _factory.LoggedAsAcademic();

        var admClient = await _factory.LoggedAsAdm();

        // Act
        var users = await admClient.GetUsers();

        // Assert
        users.Should().HaveCountGreaterThanOrEqualTo(2);
    }
}
