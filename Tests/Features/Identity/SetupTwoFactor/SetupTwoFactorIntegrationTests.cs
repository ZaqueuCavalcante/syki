namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    [TestCaseSource(nameof(Invalid2faTokens))]
    public async Task SetupTwoFactor_Should_not_setup_2fa_when_token_is_wrong(string token)
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var response = await client.SetupTwoFactor(token);

        // Assert
        response.Should().BeFalse();
        var key = await client.GetTwoFactorKey();
        key.Success.TwoFactorEnabled.Should().BeFalse();
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

    [Test]
    public async Task SetupTwoFactor_Should_record_activity_when_2fa_setup_token_is_invalid()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var response = await client.SetupTwoFactor("000000");

        // Assert
        response.Should().BeFalse();
    }

    [Test]
    public async Task SetupTwoFactor_Should_setup_2fa()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        var keyResponse = await client.GetTwoFactorKey();
        var token = keyResponse.Success.Key.GenerateTOTP();

        // Act
        var response = await client.SetupTwoFactor(token);

        // Assert
        response.Should().BeTrue();
        var key = await client.GetTwoFactorKey();
        key.Success.TwoFactorEnabled.Should().BeTrue();
    }

    [Test]
    public async Task SetupTwoFactor_Should_record_activity_log_on_setup_2fa()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        var keyResponse = await client.GetTwoFactorKey();
        var totpToken = keyResponse.Success.Key.GenerateTOTP();

        // Act
        var success = await client.SetupTwoFactor(totpToken);

        // Assert
        success.Should().BeTrue();
    }
}
