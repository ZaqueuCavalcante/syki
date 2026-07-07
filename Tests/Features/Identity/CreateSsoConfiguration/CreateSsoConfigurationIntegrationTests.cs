using Syki.Back.Features.Identity.CreateSsoConfiguration;

namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    #region Authentication

    [Test]
    public async Task Identity_CreateSsoConfiguration_Should_not_create_sso_configuration_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.CreateSsoConfiguration();

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Authorization

    [Test]
    public async Task Identity_CreateSsoConfiguration_Should_not_create_sso_configuration_when_user_has_no_permission()
    {
        // Arrange
        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.CreateSsoConfiguration();

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    #endregion

    #region Validation errors

    [Test]
    [TestCase((SsoProviderType)99)]
    public async Task Identity_CreateSsoConfiguration_Should_not_create_sso_configuration_with_invalid_provider_type(SsoProviderType providerType)
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.CreateSsoConfiguration(providerType: providerType);

        // Assert
        result.ShouldBeError(InvalidSsoProviderType.I);
    }

    [Test]
    [TestCase("")]
    public async Task Identity_CreateSsoConfiguration_Should_not_create_sso_configuration_with_invalid_authority(string authority)
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.CreateSsoConfiguration(authority: authority);

        // Assert
        result.ShouldBeError(InvalidSsoAuthority.I);
    }

    [Test]
    [TestCase("")]
    [TestCase("abc")]
    public async Task Identity_CreateSsoConfiguration_Should_not_create_sso_configuration_with_invalid_client_id(string clientId)
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.CreateSsoConfiguration(clientId: clientId);

        // Assert
        result.ShouldBeError(InvalidSsoClientId.I);
    }

    [Test]
    [TestCase("")]
    [TestCase("short")]
    public async Task Identity_CreateSsoConfiguration_Should_not_create_sso_configuration_with_invalid_client_secret(string clientSecret)
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.CreateSsoConfiguration(clientSecret: clientSecret);

        // Assert
        result.ShouldBeError(InvalidSsoClientSecret.I);
    }

    [Test]
    public async Task Identity_CreateSsoConfiguration_Should_not_create_sso_configuration_when_authority_is_not_https()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.CreateSsoConfiguration(authority: "http://login.microsoftonline.com/tenant-id/v2.0");

        // Assert
        result.ShouldBeError(SsoAuthorityMustBeHttps.I);
    }

    [Test]
    public async Task Identity_CreateSsoConfiguration_Should_not_create_sso_configuration_when_authority_has_user_info()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.CreateSsoConfiguration(authority: "https://evil.com@login.microsoftonline.com/tenant-id/v2.0");

        // Assert
        result.ShouldBeError(SsoAuthorityHasUserInfo.I);
    }

    [Test]
    public async Task Identity_CreateSsoConfiguration_Should_not_create_sso_configuration_when_authority_is_link_local()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.CreateSsoConfiguration(authority: "https://169.254.169.254/v2.0");

        // Assert
        result.ShouldBeError(SsoAuthorityLinkLocalNotAllowed.I);
    }

    [Test]
    public async Task Identity_CreateSsoConfiguration_Should_not_create_sso_configuration_when_domain_already_configured()
    {
        // Arrange
        var client = await _back.LoggedAsDirector("director@sso-duplicate-domain.com");
        await client.CreateSsoConfiguration();

        // Act
        var result = await client.CreateSsoConfiguration();

        // Assert
        result.ShouldBeError(SsoDomainAlreadyConfigured.I);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task Identity_CreateSsoConfiguration_Should_create_sso_configuration()
    {
        // Arrange
        var client = await _back.LoggedAsDirector("director@sso-happy-path.com");

        // Act
        var result = await client.CreateSsoConfiguration();

        // Assert
        var config = result.Success;
        config.Id.Should().NotBeEmpty();
    }

    #endregion
}
