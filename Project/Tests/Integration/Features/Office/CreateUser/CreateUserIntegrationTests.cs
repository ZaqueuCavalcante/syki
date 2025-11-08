using Microsoft.EntityFrameworkCore;

namespace Exato.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Should_create_exato_user_with_cs_role()
    {
        // Arrange
        var client = await _back.LoggedAsExatoAdm();

        var name = DataGen.UserName;
        var email = DataGen.Email;

        // Act
        var response = await client.CreateUser(ExatoId, name, email, ExatoCSRoleId);

        // Assert
        response.ShouldBeSuccess();
        var userId = response!.Success.Id;

        await using var ctx = _back.GetBackDbContext();
        var user = await ctx.Users.AsNoTracking().FirstAsync(x => x.Id == userId);
        user.Name.Should().Be(name);
        user.Email.Should().Be(email);
        user.CreatedAt.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMinutes(1));

        var userRole = await ctx.UserRoles.SingleAsync(x => x.UserId == userId);
        var role = await ctx.Roles.SingleAsync(x => x.Id == userRole.RoleId);
        role.Name.Should().Be("OfficeCustomerSuccess");
    }

    [Test]
    public async Task Should_not_create_exato_user_without_feature()
    {
        // Arrange
        var client = await _back.LoggedAsOfficeCS();

        // Act
        var response = await client.CreateUser(ExatoId, DataGen.UserName, DataGen.Email, ExatoAdminRoleId);

        // Assert
        response.ShouldBeError(ForbiddenErrorOut.I);
    }
}
