using System.Security.Claims;

namespace Syki.Tests.Extensions;

public class UserExtensionsUnitTests
{
    [Test]
    public void Should_get_user_id()
    {
        // Arrange
        var userIdClaim = new Claim("sub", Guid.CreateVersion7().ToString());
        var claimsIdentity = new ClaimsIdentity([userIdClaim]);
        var user = new ClaimsPrincipal(claimsIdentity);

        // Act
        var result = user.Id;

        // Assert
        result.Should().Be(userIdClaim.Value);
    }

    [Test]
    public void Should_get_user_institution_id()
    {
        // Arrange
        var institutionIdClaim = new Claim("institution", Guid.CreateVersion7().ToString());
        var claimsIdentity = new ClaimsIdentity([institutionIdClaim]);
        var user = new ClaimsPrincipal(claimsIdentity);

        // Act
        var result = user.InstitutionId;

        // Assert
        result.Should().Be(institutionIdClaim.Value);
    }
}
