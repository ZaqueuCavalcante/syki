using Syki.Back.Features.Identity.MagicLinkLogin;

namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // Validation errors
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    [Test]
    public async Task MagicLinkLogin_Should_return_error_when_token_is_empty()
    {
        // Arrange
        var client = _api.GetClient();

        // Act
        var result = await client.MagicLinkLogin("");

        // Assert
        result.ShouldBeError(InvalidMagicLinkToken.I);
    }

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // Business logic errors
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    [Test]
    public async Task MagicLinkLogin_Should_return_error_when_token_does_not_exist()
    {
        // Arrange
        var client = _api.GetClient();

        // Act
        var result = await client.MagicLinkLogin(Guid.NewGuid().ToString());

        // Assert
        result.ShouldBeError(InvalidMagicLinkToken.I);
    }
}
