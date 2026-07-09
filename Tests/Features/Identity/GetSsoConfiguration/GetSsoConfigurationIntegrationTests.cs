namespace Estud.Tests.Integration;

public partial class IntegrationTests
{
    #region Authentication

    [Test]
    public async Task Identity_GetSsoConfiguration_Should_not_get_sso_configuration_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.GetSsoConfiguration();

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Authorization

    [Test]
    public async Task Identity_GetSsoConfiguration_Should_not_get_sso_configuration_when_user_has_no_permission()
    {
        // Arrange
        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.GetSsoConfiguration();

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task Identity_GetSsoConfiguration_Should_get_sso_configuration()
    {
        // Arrange
        var client = await _back.LoggedAsDirector("director@sso-get-happy-path.com");
        var created = (await client.CreateSsoConfiguration(
            providerType: SsoProviderType.AzureAd,
            authority: "https://login.microsoftonline.com/tenant-id/v2.0",
            clientId: "00000000-0000-0000-0000-000000000000")).Success;

        // Act
        var result = await client.GetSsoConfiguration();

        // Assert
        var config = result.Success;
        config.Id.Should().Be(created.Id);
        config.ProviderType.Should().Be(SsoProviderType.AzureAd);
        config.Authority.Should().Be("https://login.microsoftonline.com/tenant-id/v2.0");
        config.ClientId.Should().Be("00000000-0000-0000-0000-000000000000");
        config.IsActive.Should().BeTrue();
        config.RequireSso.Should().BeFalse();
    }

    [Test]
    public async Task Identity_GetSsoConfiguration_Should_get_null_when_no_sso_configuration_exists()
    {
        // Arrange
        var client = await _back.LoggedAsDirector("director@sso-get-empty.com");

        // Act
        var result = await client.GetSsoConfiguration();

        // Assert
        result.Success.Should().BeNull();
    }

    #endregion
}
