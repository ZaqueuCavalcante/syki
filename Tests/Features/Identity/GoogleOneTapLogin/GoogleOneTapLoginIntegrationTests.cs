
using Estud.Back.Google;
using Estud.Back.Settings;
using Microsoft.Extensions.DependencyInjection;
using Estud.Back.Features.Identity.GoogleOneTapLogin;

namespace Estud.Tests.Integration;

public partial class IntegrationTests
{
    #region Validation errors

    [Test]
    [TestCase("")]
    [TestCase(" ")]
    [TestCase(null)]
    public async Task Identity_GoogleOneTapLogin_Should_not_login_with_empty_credential(string? credential)
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.GoogleOneTapLogin(credential);

        // Assert
        result.ShouldBeError(GoogleOneTapLoginInvalidToken.I);
    }

    [Test]
    public async Task Identity_GoogleOneTapLogin_Should_not_login_with_invalid_credential()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.GoogleOneTapLogin(Guid.NewGuid().ToString());

        // Assert
        result.ShouldBeError(GoogleOneTapLoginInvalidToken.I);
    }

    [Test]
    public async Task Identity_GoogleOneTapLogin_Should_not_login_when_email_not_verified()
    {
        // Arrange
        var client = _back.GetTestsClient();
        var credential = Guid.NewGuid().ToString();
        FakeGoogleService.SeedGoogleToken(credential, DataGen.Email, emailVerified: false);

        // Act
        var result = await client.GoogleOneTapLogin(credential);

        // Assert
        result.ShouldBeError(SocialLoginEmailNotVerified.I);
    }

    [Test]
    public async Task Identity_GoogleOneTapLogin_Should_not_login_when_email_domain_requires_sso()
    {
        // Arrange — an active SSO configuration that requires SSO for its domain
        var domain = $"sso-required-{DataGen.Numbers}.com";
        var director = await _back.LoggedAsDirector($"director@{domain}");

        var config = await director.CreateSsoConfiguration().Success();
        await director.UpdateSsoConfiguration(config.Id, requireSso: true);

        var client = _back.GetTestsClient();
        var credential = Guid.NewGuid().ToString();
        FakeGoogleService.SeedGoogleToken(credential, $"someone@{domain}");

        // Act
        var result = await client.GoogleOneTapLogin(credential);

        // Assert
        result.ShouldBeError(SocialLoginSsoRequired.I);
    }

    [Test]
    [NonParallelizable] // muta o singleton SocialLoginSettings — não pode rodar junto com outros logins sociais
    public async Task Identity_GoogleOneTapLogin_Should_not_login_when_google_one_tap_is_disabled()
    {
        // Arrange
        var client = _back.GetTestsClient();
        var settings = _back.Services.GetRequiredService<SocialLoginSettings>();

        // Act
        settings.Google.Enabled = false;
        try
        {
            var result = await client.GoogleOneTapLogin(Guid.NewGuid().ToString());

            // Assert
            result.ShouldBeError(GoogleOneTapLoginDisabled.I);
        }
        finally
        {
            settings.Google.Enabled = true;
        }
    }

    #endregion

    #region Happy path

    [Test]
    public async Task Identity_GoogleOneTapLogin_Should_login_and_auto_provision_new_user()
    {
        // Arrange
        var client = _back.GetTestsClient();
        var email = DataGen.Email;
        var credential = Guid.NewGuid().ToString();
        var subject = FakeGoogleService.SeedGoogleToken(credential, email, name: "Ada Lovelace");

        // Act
        var result = await client.GoogleOneTapLogin(credential);

        // Assert
        var login = result.Success;
        login.UserId.Should().BeGreaterThan(0);
        login.InstitutionId.Should().BeGreaterThan(0);

        await using var ctx = _back.GetDbContext();

        var institutionExists = await ctx.Institutions.AnyAsync(i => i.Id == login.InstitutionId);
        institutionExists.Should().BeTrue();

        var user = await ctx.Users.AsNoTracking().FirstAsync(u => u.Email == email);
        user.Id.Should().Be(login.UserId);
        user.Name.Should().Be("Ada Lovelace");
        user.EmailConfirmed.Should().BeTrue();

        var hasLink = await ctx.UserSocialLogins.AnyAsync(x => x.UserId == login.UserId && x.ProviderKey == subject);
        hasLink.Should().BeTrue();
    }

    [Test]
    public async Task Identity_GoogleOneTapLogin_Should_login_returning_user_with_existing_link()
    {
        // Arrange
        var client = _back.GetTestsClient();
        var email = DataGen.Email;
        var subject = Guid.NewGuid().ToString();

        var firstCredential = Guid.NewGuid().ToString();
        FakeGoogleService.SeedGoogleToken(firstCredential, email, subject: subject);
        var first = (await client.GoogleOneTapLogin(firstCredential)).Success;

        var secondCredential = Guid.NewGuid().ToString();
        FakeGoogleService.SeedGoogleToken(secondCredential, email, subject: subject);

        // Act — same Google account (same subject) logging in again
        var result = await client.GoogleOneTapLogin(secondCredential);

        // Assert — same user and institution reused, single social login link
        var second = result.Success;
        second.UserId.Should().Be(first.UserId);
        second.InstitutionId.Should().Be(first.InstitutionId);

        await using var ctx = _back.GetDbContext();
        var links = await ctx.UserSocialLogins.CountAsync(x => x.UserId == first.UserId);
        links.Should().Be(1);
    }

    [Test]
    public async Task Identity_GoogleOneTapLogin_Should_link_and_login_existing_user_by_email()
    {
        // Arrange — user already exists (registered, e-mail not yet confirmed)
        var registerClient = _back.GetTestsClient();
        var email = DataGen.Email;
        var existing = await registerClient.RegisterUser(email).Success();

        var client = _back.GetTestsClient();
        var credential = Guid.NewGuid().ToString();
        var subject = FakeGoogleService.SeedGoogleToken(credential, email);

        // Act
        var result = await client.GoogleOneTapLogin(credential);

        // Assert — same user and institution reused, account linked and e-mail confirmed
        var login = result.Success;
        login.UserId.Should().Be(existing.Id);
        login.InstitutionId.Should().Be(existing.InstitutionId);

        await using var ctx = _back.GetDbContext();
        var user = await ctx.Users.AsNoTracking().FirstAsync(u => u.Id == existing.Id);
        user.EmailConfirmed.Should().BeTrue();

        var hasLink = await ctx.UserSocialLogins.AnyAsync(x =>
            x.UserId == existing.Id && x.Provider == SocialLoginProvider.Google && x.ProviderKey == subject);
        hasLink.Should().BeTrue();
    }

    [Test]
    public async Task Identity_GoogleOneTapLogin_Should_use_email_as_name_when_name_is_empty()
    {
        // Arrange
        var client = _back.GetTestsClient();
        var email = DataGen.Email;
        var credential = Guid.NewGuid().ToString();
        FakeGoogleService.SeedGoogleToken(credential, email, name: null);

        // Act
        var result = await client.GoogleOneTapLogin(credential);

        // Assert
        var login = result.Success;
        await using var ctx = _back.GetDbContext();
        var user = await ctx.Users.AsNoTracking().FirstAsync(u => u.Id == login.UserId);
        user.Name.Should().Be(email);
    }

    [Test]
    public async Task Identity_GoogleOneTapLogin_Should_match_existing_user_ignoring_email_case()
    {
        // Arrange — existing user stored in lowercase
        var registerClient = _back.GetTestsClient();
        var email = DataGen.Email;
        var existing = await registerClient.RegisterUser(email).Success();

        var client = _back.GetTestsClient();
        var credential = Guid.NewGuid().ToString();
        FakeGoogleService.SeedGoogleToken(credential, email.ToUpperInvariant());

        // Act — Google returns the same e-mail in a different case
        var result = await client.GoogleOneTapLogin(credential);

        // Assert — matched the existing user and its institution, no duplicate
        result.Success.UserId.Should().Be(existing.Id);
        result.Success.InstitutionId.Should().Be(existing.InstitutionId);
    }

    #endregion

    #region Security

    [Test]
    public async Task Identity_GoogleOneTapLogin_Should_not_link_existing_user_when_email_not_verified()
    {
        // Arrange — existing unconfirmed user
        var registerClient = _back.GetTestsClient();
        var email = DataGen.Email;
        var existing = await registerClient.RegisterUser(email).Success();

        var client = _back.GetTestsClient();
        var credential = Guid.NewGuid().ToString();
        FakeGoogleService.SeedGoogleToken(credential, email, emailVerified: false);

        // Act
        var result = await client.GoogleOneTapLogin(credential);

        // Assert — rejected, no side effects (no link, e-mail still not confirmed)
        result.ShouldBeError(SocialLoginEmailNotVerified.I);

        await using var ctx = _back.GetDbContext();
        var user = await ctx.Users.AsNoTracking().FirstAsync(u => u.Id == existing.Id);
        user.EmailConfirmed.Should().BeFalse();

        var hasLink = await ctx.UserSocialLogins.AnyAsync(x => x.UserId == existing.Id);
        hasLink.Should().BeFalse();
    }

    [Test]
    public async Task Identity_GoogleOneTapLogin_Should_login_by_provider_key_even_when_email_changed()
    {
        // Arrange — account first provisioned with email A and subject S
        var client = _back.GetTestsClient();
        var emailA = DataGen.Email;
        var subject = Guid.NewGuid().ToString();

        var firstCredential = Guid.NewGuid().ToString();
        FakeGoogleService.SeedGoogleToken(firstCredential, emailA, subject: subject);
        var first = (await client.GoogleOneTapLogin(firstCredential)).Success;

        // Same Google account (subject S) but now reporting a different e-mail B
        var emailB = DataGen.Email;
        var secondCredential = Guid.NewGuid().ToString();
        FakeGoogleService.SeedGoogleToken(secondCredential, emailB, subject: subject);

        // Act
        var result = await client.GoogleOneTapLogin(secondCredential);

        // Assert — identity is anchored to the provider key, not the e-mail
        result.Success.UserId.Should().Be(first.UserId);

        await using var ctx = _back.GetDbContext();
        var createdForEmailB = await ctx.Users.AnyAsync(u => u.Email == emailB);
        createdForEmailB.Should().BeFalse();
    }

    #endregion
}
