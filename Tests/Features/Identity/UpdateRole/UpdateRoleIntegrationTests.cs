using Estud.Back.Domain.Identity;

namespace Estud.Tests.Integration;

public partial class IntegrationTests
{
    #region Authentication

    [Test]
    public async Task Identity_UpdateRole_Should_not_update_role_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.UpdateRole(1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Authorization

    [Test]
    public async Task Identity_UpdateRole_Should_not_update_role_when_user_has_no_permission()
    {
        // Arrange
        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.UpdateRole(1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    #endregion

    #region Validation errors

    [Test]
    [TestCase("")]
    public async Task Identity_UpdateRole_Should_not_update_role_with_invalid_name(string name)
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.UpdateRole(1, name: name);

        // Assert
        result.ShouldBeError(InvalidRoleName.I);
    }

    [Test]
    [TestCase("")]
    public async Task Identity_UpdateRole_Should_not_update_role_with_invalid_description(string description)
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.UpdateRole(1, description: description);

        // Assert
        result.ShouldBeError(InvalidRoleDescription.I);
    }

    [Test]
    public async Task Identity_UpdateRole_Should_not_update_role_with_invalid_permissions()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.UpdateRole(1, permissions: [99999]);

        // Assert
        result.ShouldBeError(InvalidPermissionsList.I);
    }

    [Test]
    public async Task Identity_UpdateRole_Should_not_update_role_with_permissions_not_allowed_for_the_base_type()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var role = await client.CreateRole(name: "Professor Substituto", baseType: UserType.Teacher, permissions: []).Success();

        // Act
        var result = await client.UpdateRole(role.Id, name: "Professor Substituto", permissions: [EstudPermissions.ManageRoles.Id]);

        // Assert
        result.ShouldBeError(InvalidPermissionsForUserType.I);
    }

    [Test]
    public async Task Identity_UpdateRole_Should_not_update_role_when_it_does_not_exist()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.UpdateRole(999999);

        // Assert
        result.ShouldBeError(RoleNotFound.I);
    }

    [Test]
    public async Task Identity_UpdateRole_Should_not_update_role_when_name_already_exists()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        await client.CreateRole(name: "Admin");
        var editor = await client.CreateRole(name: "Editor").Success();

        // Act
        var result = await client.UpdateRole(editor.Id, name: "Admin");

        // Assert
        result.ShouldBeError(RoleNameAlreadyExists.I);
    }

    [Test]
    public async Task Identity_UpdateRole_Should_not_update_role_that_has_more_permissions_than_the_user_has()
    {
        // Arrange
        var email = DataGen.Email;
        var director = await _back.LoggedAsDirector(email);

        var limitedRoleResult = await director.CreateRole(name: "Gerente de Perfis", permissions: [EstudPermissions.ManageRoles.Id]).Success();
        var limitedRoleId = limitedRoleResult.Id;
        var powerfulRoleResult = await director.CreateRole(name: "Poderosa", permissions: [EstudPermissions.ManageRoles.Id, EstudPermissions.ManageSso.Id]).Success();
        var powerfulRoleId = powerfulRoleResult.Id;
        var userId = director.User.Id;

        await using (var ctx = _back.GetDbContext())
        {
            var userRole = await ctx.UserRoles.FirstAsync(x => x.UserId == userId);
            ctx.Remove(userRole);
            ctx.Add(new EstudUserRole(userRole.InstitutionId, userRole.UserId, limitedRoleId));
            await ctx.SaveChangesAsync();
        }

        var client = await _back.LoginAs(email);

        // Act
        var result = await client.UpdateRole(powerfulRoleId, name: "Poderosa", permissions: [EstudPermissions.ManageRoles.Id]);

        // Assert
        result.ShouldBeError(InvalidRolePermissions.I);
    }

    [Test]
    public async Task Identity_UpdateRole_Should_not_update_role_with_more_permissions_than_the_user_has()
    {
        // Arrange
        var email = DataGen.Email;
        var director = await _back.LoggedAsDirector(email);

        var limitedRoleResult = await director.CreateRole(name: "Gerente de Perfis", permissions: [EstudPermissions.ManageRoles.Id]).Success();
        var limitedRoleId = limitedRoleResult.Id;
        var editableRoleResult = await director.CreateRole(name: "Editável", permissions: [EstudPermissions.ManageRoles.Id]).Success();
        var editableRoleId = editableRoleResult.Id;
        var userId = director.User.Id;

        await using (var ctx = _back.GetDbContext())
        {
            var userRole = await ctx.UserRoles.FirstAsync(x => x.UserId == userId);
            ctx.Remove(userRole);
            ctx.Add(new EstudUserRole(userRole.InstitutionId, userRole.UserId, limitedRoleId));
            await ctx.SaveChangesAsync();
        }

        var client = await _back.LoginAs(email);

        // Act
        var result = await client.UpdateRole(editableRoleId, name: "Editável", permissions: [EstudPermissions.ManageRoles.Id, EstudPermissions.ManageSso.Id]);

        // Assert
        result.ShouldBeError(InvalidRolePermissions.I);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task Identity_UpdateRole_Should_update_role()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var role = await client.CreateRole(name: "Admin", description: "Administrador", baseType: UserType.Manager, permissions: []).Success();

        // Act
        var result = await client.UpdateRole(role.Id, name: "Gestor", description: "Gestor acadêmico", permissions: []);

        // Assert
        result.Success.Id.Should().Be(role.Id);

        var updated = await client.GetRole(role.Id).Success();
        updated.Name.Should().Be("Gestor");
        updated.Description.Should().Be("Gestor acadêmico");
    }

    #endregion
}
