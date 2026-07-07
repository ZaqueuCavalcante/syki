namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    #region Authentication

    [Test]
    public async Task Identity_UpdateSsoConfiguration_Should_not_update_sso_configuration_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.UpdateSsoConfiguration(Guid.NewGuid());

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Authorization

    [Test]
    public async Task Identity_UpdateSsoConfiguration_Should_not_update_sso_configuration_when_user_has_no_permission()
    {
        // Arrange
        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.UpdateSsoConfiguration(Guid.NewGuid());

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    #endregion

    #region Validation errors

    [Test]
    [TestCase((SsoProviderType)99)]
    public async Task Identity_UpdateSsoConfiguration_Should_not_update_sso_configuration_with_invalid_provider_type(SsoProviderType providerType)
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.UpdateSsoConfiguration(Guid.NewGuid(), providerType: providerType);

        // Assert
        result.ShouldBeError(InvalidSsoProviderType.I);
    }

    [Test]
    [TestCase("")]
    public async Task Identity_UpdateSsoConfiguration_Should_not_update_sso_configuration_with_invalid_authority(string authority)
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.UpdateSsoConfiguration(Guid.NewGuid(), authority: authority);

        // Assert
        result.ShouldBeError(InvalidSsoAuthority.I);
    }

    [Test]
    [TestCase("")]
    [TestCase("abc")]
    public async Task Identity_UpdateSsoConfiguration_Should_not_update_sso_configuration_with_invalid_client_id(string clientId)
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.UpdateSsoConfiguration(Guid.NewGuid(), clientId: clientId);

        // Assert
        result.ShouldBeError(InvalidSsoClientId.I);
    }

    [Test]
    public async Task Identity_UpdateSsoConfiguration_Should_not_update_sso_configuration_when_authority_is_not_https()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.UpdateSsoConfiguration(Guid.NewGuid(), authority: "http://login.microsoftonline.com/tenant-id/v2.0");

        // Assert
        result.ShouldBeError(SsoAuthorityMustBeHttps.I);
    }

    [Test]
    public async Task Identity_UpdateSsoConfiguration_Should_not_update_sso_configuration_when_authority_has_user_info()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.UpdateSsoConfiguration(Guid.NewGuid(), authority: "https://evil.com@login.microsoftonline.com/tenant-id/v2.0");

        // Assert
        result.ShouldBeError(SsoAuthorityHasUserInfo.I);
    }

    [Test]
    public async Task Identity_UpdateSsoConfiguration_Should_not_update_sso_configuration_when_authority_is_link_local()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.UpdateSsoConfiguration(Guid.NewGuid(), authority: "https://169.254.169.254/v2.0");

        // Assert
        result.ShouldBeError(SsoAuthorityLinkLocalNotAllowed.I);
    }

    [Test]
    public async Task Identity_UpdateSsoConfiguration_Should_not_update_sso_configuration_when_it_does_not_exist()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.UpdateSsoConfiguration(Guid.NewGuid());

        // Assert
        result.ShouldBeError(SsoConfigurationNotFound.I);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task Identity_UpdateSsoConfiguration_Should_update_sso_configuration()
    {
        // Arrange
        var client = await _back.LoggedAsDirector("director@sso-update-happy-path.com");
        var config = (await client.CreateSsoConfiguration()).Success;

        // Act
        var result = await client.UpdateSsoConfiguration(
            config.Id,
            providerType: SsoProviderType.Okta,
            authority: "https://dev-123456.okta.com",
            clientId: "0oa1b2c3d4e5f6g7h8i9",
            clientSecret: "new-client-secret-value",
            isActive: false,
            requireSso: true);

        // Assert
        var updated = result.Success;
        updated.Id.Should().Be(config.Id);
    }

    #endregion
}
