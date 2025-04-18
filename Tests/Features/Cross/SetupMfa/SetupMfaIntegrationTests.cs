namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_setup_mfa()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();

        var keyResponse = await client.Http.GetMfaKey();
        var token = keyResponse.Key.GenerateTOTP();

        // Act
        var response = await client.Http.SetupMfa(token);

        // Assert
        response.Should().BeTrue();
    }

    [Test]
    [TestCaseSource(typeof(TestData), nameof(TestData.InvalidMfaTokens))]
    public async Task Should_not_setup_mfa_when_token_is_wrong(string token)
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();

        // Act
        var response = await client.Http.SetupMfa(token);

        // Assert
        response.Should().BeFalse();
    }
}
