namespace Exato.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Should_setup_2fa()
    {
        // Arrange
        var client = await _back.LoggedAsOfficeCS();

        var keyResponse = await client.Cross.GetTwoFactorAuthenticationKey();
        var token = keyResponse.Key.GenerateTOTP();

        // Act
        var response = await client.Cross.SetupTwoFactorAuthentication(token);

        // Assert
        response.Should().BeTrue();
        var key = await client.Cross.GetTwoFactorAuthenticationKey();
        key.TwoFactorEnabled.Should().BeTrue();
    }

    [Test]
    [TestCaseSource(nameof(Invalid2faTokens))]
    public async Task Should_not_setup_2fa_when_token_is_wrong(string token)
    {
        // Arrange
        var client = await _back.LoggedAsOfficeCS();

        // Act
        var response = await client.Cross.SetupTwoFactorAuthentication(token);

        // Assert
        response.Should().BeFalse();
        var key = await client.Cross.GetTwoFactorAuthenticationKey();
        key.TwoFactorEnabled.Should().BeFalse();
    }

    private static IEnumerable<object[]> Invalid2faTokens()
    {
        foreach (var token in new List<string>()
        {
            "",
            " ",
            "5464",
            "exato",
            "123456",
            "lalal.com",
            "5816811681816",
        })
        {
            yield return [token];
        }
    }
}
