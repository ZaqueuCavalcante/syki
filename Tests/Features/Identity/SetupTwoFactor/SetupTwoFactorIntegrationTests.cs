namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    #region Authentication

    [Test]
    public async Task Identity_SetupTwoFactor_Should_not_setup_2fa_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var response = await client.SetupTwoFactor("000000");

        // Assert
        response.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Validation errors

    [Test]
    [TestCaseSource(nameof(Invalid2faTokens))]
    public async Task SetupTwoFactor_Should_not_setup_2fa_when_token_is_wrong(string token)
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var response = await client.SetupTwoFactor(token);

        // Assert
        response.ShouldBeError(InvalidTwoFactorToken.I);
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

    #endregion

    #region Happy path

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
        response.ShouldBeSuccess();
        var key = await client.GetTwoFactorKey();
        key.Success.TwoFactorEnabled.Should().BeTrue();
    }

    #endregion
}
