namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    [TestCaseSource(typeof(TestData), nameof(TestData.InvalidMfaTokens))]
    public async Task Should_not_setup_mfa_when_token_is_wrong(string token)
    {
        // Arrange
        var client = await _factory.LoggedAsAcademico();

        // Act
        var response = await client.SetupMfa(token);

        // Assert
        response.Should().BeFalse();
    }

    [Test]
    public async Task Should_setup_mfa()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademico();

        var keyResponse = await client.GetMfaKey();
        var token = keyResponse.Key.ToMfaToken();

        // Act
        var response = await client.SetupMfa(token);

        // Assert
        response.Should().BeTrue();
    }
}
