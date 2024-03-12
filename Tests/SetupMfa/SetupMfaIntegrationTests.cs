namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    [TestCaseSource(typeof(TestData), nameof(TestData.InvalidMfaTokens))]
    public async Task Should_not_setup_mfa_when_token_is_wrong(string token)
    {
        // Arrange
        var client = _factory.GetClient();
        var user = await client.RegisterUser(_factory);
        await client.Login(user.Email, user.Password);

        // Act
        var response = await client.SetupMfa(token);

        // Assert
        response.Should().BeFalse();
    }

    [Test]
    public async Task Should_setup_mfa()
    {
        // Arrange
        var client = _factory.GetClient();
        var user = await client.RegisterUser(_factory);
        await client.Login(user.Email, user.Password);

        var keyResponse = await client.GetMfaKey();
        var token = keyResponse.Key.ToMfaToken();

        // Act
        var response = await client.SetupMfa(token);

        // Assert
        response.Should().BeTrue();
    }
}
