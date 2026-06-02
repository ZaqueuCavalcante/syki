namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    #region Authentication

    [Test]
    public async Task Identity_CreateRole_Should_not_create_role_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.CreateRole();

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Authorization

    [Test]
    public async Task Identity_CreateRole_Should_not_create_role_when_user_has_no_permission()
    {
        // Arrange
        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.CreateRole();

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    #endregion

    #region Validation errors

    [Test]
    [TestCase("")]
    public async Task Identity_CreateRole_Should_not_create_role_with_invalid_name(string name)
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.CreateRole(name: name);

        // Assert
        result.ShouldBeError(InvalidRoleName.I);
    }

    [Test]
    [TestCase("")]
    public async Task Identity_CreateRole_Should_not_create_role_with_invalid_description(string description)
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.CreateRole(description: description);

        // Assert
        result.ShouldBeError(InvalidRoleDescription.I);
    }

    [Test]
    [TestCase((UserType)99)]
    public async Task Identity_CreateRole_Should_not_create_role_with_invalid_base_type(UserType baseType)
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.CreateRole(baseType: baseType);

        // Assert
        result.ShouldBeError(InvalidRoleBaseType.I);
    }

    [Test]
    public async Task Identity_CreateRole_Should_not_create_role_with_invalid_permissions()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.CreateRole(permissions: [99999]);

        // Assert
        result.ShouldBeError(InvalidPermissionsList.I);
    }

    #endregion

    #region Domain errors

    [Test]
    public async Task Identity_CreateRole_Should_not_create_role_when_name_already_exists()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        await client.CreateRole(name: "Admin");

        // Act
        var result = await client.CreateRole(name: "Admin");

        // Assert
        result.ShouldBeError(RoleNameAlreadyExists.I);
    }

    [Test]
    public async Task Identity_CreateRole_Should_not_create_role_when_permissions_are_not_subset()
    {
        // Arrange
        var client = await _back.LoggedAs("Gestor", [SykiPermissions.ManageRoles]);

        // Act
        var result = await client.CreateRole(permissions: [SykiPermissions.ManageCampi.Id]);

        // Assert
        result.ShouldBeError(InvalidRolePermissions.I);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task Identity_CreateRole_Should_create_role()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.CreateRole(name: "Admin", description: "Administrador", baseType: UserType.Manager, permissions: []);

        // Assert
        var role = result.Success;
        role.Id.Should().NotBe(0);
    }

    #endregion
}
