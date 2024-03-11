using Syki.Back.Extensions;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

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
        var result = user.InstitutionId();

        // Assert
        result.Should().Be(faculdadeIdClaim.Value);
    }

    [Test]
    [TestCaseSource(typeof(TestData), nameof(TestData.AuditPaths))]
    public void Shoud_return_if_path_is_login((PathString path, bool isAuditable) data)
    {
        // Arrange / Act
        var result = data.path.IsAuditable();

        // Assert
        result.Should().Be(data.isAuditable);
    }
}
