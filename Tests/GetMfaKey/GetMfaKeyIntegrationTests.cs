namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    [TestCaseSource(typeof(TestData), nameof(TestData.AllUsersRoles))]
    public async Task Should_get_mfa_key_for_all_user_roles(string role)
    {
        // Arrange
        var client = _factory.GetClient();
        var user = await client.RegisterUser(_factory);
        await client.Login(user.Email, user.Password);

        // Act
        var response = await client.GetMfaKey();

        // Assert
        response.Key.Should().HaveLength(32);
    }

    [Test]
    [TestCaseSource(typeof(TestData), nameof(TestData.AllUsersRoles))]
    public async Task Should_get_same_mfa_key_for_many_gets(string role)
    {
        // Arrange
        var client = _factory.GetClient();
        var user = await client.RegisterUser(_factory);
        await client.Login(user.Email, user.Password);

        // Act
        var response00 = await client.GetMfaKey();
        var response01 = await client.GetMfaKey();
        var response02 = await client.GetMfaKey();

        // Assert
        response00.Key.Should().HaveLength(32);
        response00.Key.Should().Be(response01.Key);
        response00.Key.Should().Be(response02.Key);
    }
}
