using Microsoft.EntityFrameworkCore;

namespace Exato.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Should_create_exato_role()
    {
        // Arrange
        var client = await _back.LoggedAsExatoAdm();

        var name = "RoleDeTeste";
        var description = "Role de teste";

        // Act
        var response = await client.CreateRole(name, description, ExatoId, [5, 10, 26]);

        // Assert
        response.ShouldBeSuccess();
        var roleId = response!.Success.Id;

        await using var ctx = _back.GetBackDbContext();
        var role = await ctx.Roles.SingleAsync(x => x.Id == roleId);
        role.Name.Should().Be(name);
        role.Description.Should().Be(description);
        role.OrganizationId.Should().Be(ExatoId);
        role.Features.Should().BeEquivalentTo([5, 10, 26]);
    }

    [Test]
    public async Task Should_not_create_exato_role_without_feature()
    {
        // Arrange
        var client = await _back.LoggedAsOfficeCS();

        // Act
        var response = await client.CreateRole("name", "description", ExatoId, [5, 10, 26]);

        // Assert
        response.ShouldBeError(ForbiddenErrorOut.I);
    }
}
