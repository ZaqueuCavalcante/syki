namespace Syki.Tests.Integration;

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
    [TestCase((UserType)99)]
    public async Task Identity_UpdateRole_Should_not_update_role_with_invalid_base_type(UserType baseType)
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.UpdateRole(1, baseType: baseType);

        // Assert
        result.ShouldBeError(InvalidRoleBaseType.I);
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
        var editor = (await client.CreateRole(name: "Editor")).Success;

        // Act
        var result = await client.UpdateRole(editor.Id, name: "Admin");

        // Assert
        result.ShouldBeError(RoleNameAlreadyExists.I);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task Identity_UpdateRole_Should_update_role()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var role = (await client.CreateRole(name: "Admin", description: "Administrador", baseType: UserType.Manager, permissions: [])).Success;

        // Act
        var result = await client.UpdateRole(role.Id, name: "Gestor", description: "Gestor acadêmico", baseType: UserType.Manager, permissions: []);

        // Assert
        result.Success.Id.Should().Be(role.Id);

        var updated = (await client.GetRole(role.Id)).Success;
        updated.Name.Should().Be("Gestor");
        updated.Description.Should().Be("Gestor acadêmico");
    }

    #endregion
}
