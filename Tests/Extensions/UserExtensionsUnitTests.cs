using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Syki.Tests.Extensions;

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
    public void Shoud_get_user_institution_id()
    {
        // Arrange
        var institutionIdClaim = new Claim("institution", Guid.NewGuid().ToString());
        var claimsIdentity = new ClaimsIdentity([institutionIdClaim]);
        var user = new ClaimsPrincipal(claimsIdentity);

        // Act
        var result = user.InstitutionId();

        // Assert
        result.Should().Be(institutionIdClaim.Value);
    }

    [Test]
    [TestCaseSource(typeof(TestData), nameof(TestData.AuditPaths))]
    public void Shoud_return_if_path_is_auditable((PathString path, bool isAuditable) x)
    {
        // Arrange / Act
        var result = x.path.IsAuditable();

        // Assert
        result.Should().Be(x.isAuditable);
    }
}
