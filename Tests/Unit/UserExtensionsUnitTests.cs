using NUnit.Framework;
using FluentAssertions;
using Syki.Back.Extensions;
using System.Security.Claims;

namespace Syki.Tests.Unit;

public class UserExtensionsUnitTests
{
    [Test]
    public void Shoud_get_user_id()
    {
        // Arrange
        var userIdClaim = new Claim("sub", Guid.NewGuid().ToString());
        var claimsIdentity = new ClaimsIdentity([userIdClaim]);
        var user = new ClaimsPrincipal(claimsIdentity);

        // Act
        var result = user.Id();

        // Assert
        result.Should().Be(userIdClaim.Value);
    }

    [Test]
    public void Shoud_get_user_fauldade_id()
    {
        // Arrange
        var faculdadeIdClaim = new Claim("faculdade", Guid.NewGuid().ToString());
        var claimsIdentity = new ClaimsIdentity([faculdadeIdClaim]);
        var user = new ClaimsPrincipal(claimsIdentity);

        // Act
        var result = user.Facul();

        // Assert
        result.Should().Be(faculdadeIdClaim.Value);
    }
}
