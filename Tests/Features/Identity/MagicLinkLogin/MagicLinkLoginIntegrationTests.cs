using Syki.Tests.Integration.Base;
using Syki.Back.Features.Users.RegisterUser;
using Syki.Back.Features.Identity.MagicLinkLogin;

namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // Validation errors
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    [Test]
    public async Task Identity_MagicLinkLogin_Should_return_error_when_token_is_empty()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.MagicLinkLogin("");

        // Assert
        result.ShouldBeError(InvalidMagicLink.I);
    }

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // Business logic errors
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    [Test]
    public async Task Identity_MagicLinkLogin_Should_return_error_when_token_does_not_exist()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.MagicLinkLogin(Guid.NewGuid().ToString());

        // Assert
        result.ShouldBeError(InvalidMagicLink.I);
    }

    [Test]
    public async Task Identity_MagicLinkLogin_Should_return_error_when_token_is_already_used()
    {
        // Arrange
        var client = _back.GetTestsClient();
        var email = DataGen.Email;
        await client.RegisterUser(email);
        await _back.AwaitCommandsProcessing();

        var token = await _back.GetMagicLink(email);
        token.Should().NotBeNull();

        // Use the token once
        var firstResult = await client.MagicLinkLogin(token!);
        firstResult.ShouldBeSuccess();

        // Act - try to use it again
        var result = await client.MagicLinkLogin(token!);

        // Assert
        result.ShouldBeError(InvalidMagicLink.I);
    }

    [Test]
    public async Task Identity_MagicLinkLogin_Should_return_error_when_token_is_expired()
    {
        // Arrange
        var client = _back.GetTestsClient();
        var email = DataGen.Email;
        await client.RegisterUser(email);
        await _back.AwaitCommandsProcessing();

        var token = await _back.GetMagicLink(email);
        token.Should().NotBeNull();

        // Expire the token manually
        await using var ctx = _back.GetDbContext();
        var magicLink = await ctx.WebMagicLinks.FirstAsync(t => t.Id == Guid.Parse(token!));
        magicLink.ExpiresAt = DateTime.UtcNow.AddHours(-1);
        await ctx.SaveChangesAsync();

        // Act
        var result = await client.MagicLinkLogin(token!);

        // Assert
        result.ShouldBeError(InvalidMagicLink.I);
    }

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // Happy path
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    [Test]
    public async Task Identity_MagicLinkLogin_Should_login_successfully_with_valid_token()
    {
        // Arrange
        var client = _back.GetTestsClient();
        var email = DataGen.Email;
        await client.RegisterUser(email);
        await _back.AwaitCommandsProcessing();

        var token = await _back.GetMagicLink(email);
        token.Should().NotBeNull();

        // Act
        var result = await client.MagicLinkLogin(token!);

        // Assert
        result.ShouldBeSuccess();
        result.Success.UserId.Should().NotBe(0);
        result.Success.InstitutionId.Should().NotBe(0);
    }

    [Test]
    public async Task Identity_MagicLinkLogin_Should_set_email_confirmed_to_true()
    {
        // Arrange
        var client = _back.GetTestsClient();
        var email = DataGen.Email;
        await client.RegisterUser(email);
        await _back.AwaitCommandsProcessing();

        var token = await _back.GetMagicLink(email);
        token.Should().NotBeNull();

        // Verify EmailConfirmed is false before magic link login
        await using (var ctxBefore = _back.GetDbContext())
        {
            var userBefore = await ctxBefore.Users.AsNoTracking().FirstAsync(x => x.Email == email);
            userBefore.EmailConfirmed.Should().BeFalse();
        }

        // Act
        var result = await client.MagicLinkLogin(token!);

        // Assert
        result.ShouldBeSuccess();

        await using var ctx = _back.GetDbContext();
        var user = await ctx.Users.AsNoTracking().FirstAsync(x => x.Email == email);
        user.EmailConfirmed.Should().BeTrue();
    }

    [Test]
    public async Task Identity_MagicLinkLogin_Should_enqueue_magic_link_command_on_registration()
    {
        // Arrange
        var client = _back.GetTestsClient();
        var email = DataGen.Email;

        // Act
        var response = await client.RegisterUser(email);

        // Assert
        response.ShouldBeSuccess();

        await using var ctx = _back.GetDbContext();
        var command = await ctx.Commands.AsNoTracking()
            .FirstAsync(c => c.Type == nameof(SendFirstAccessMagicLinkEmailCommand) && c.Data.Contains(email));
        command.MaxRetries.Should().Be(1);

        await _back.AwaitCommandsProcessing();

        var emailService = _back.GetFakeEmailsService();
        var emailEntry = emailService.FirstAccessMagicLinkEmails.FirstOrDefault(e => e.Contains(email));
        emailEntry.Should().NotBeNull();
        emailEntry.Should().Contain("/magic-link?token=");
    }
}
