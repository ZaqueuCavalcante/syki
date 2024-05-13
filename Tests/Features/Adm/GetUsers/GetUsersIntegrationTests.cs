namespace Syki.Tests.Integration;

public class GetUsersIntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Should_return_all_users()
    {
        // Arrange
        var client = _factory.GetClient();
        var user = await client.RegisterUser(_factory);

        var admClient = await _factory.LoggedAsAdm();

        // Act
        var users = await admClient.GetAsync<List<UserOut>>("/adm/users");

        // Assert
        users.Should().HaveCount(2);
        var academic = users.First(x => x.Role == "Academic");
        academic.Institution.Should().EndWith(user.Email);
    }
}
