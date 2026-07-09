namespace Estud.Tests.Integration;

public partial class IntegrationTests
{
    #region Authentication

    [Test]
    public async Task Identity_GetRole_Should_not_get_role_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.GetRole(1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Authorization

    [Test]
    public async Task Identity_GetRole_Should_not_get_role_when_user_has_no_permission()
    {
        // Arrange
        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.GetRole(1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    #endregion

    #region Validation errors

    [Test]
    public async Task Identity_GetRole_Should_not_get_role_when_it_does_not_exist()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.GetRole(999999);

        // Assert
        result.ShouldBeError(RoleNotFound.I);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task Identity_GetRole_Should_get_the_role()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var role = (await client.CreateRole(name: "Admin", description: "Administrador", baseType: UserType.Manager, permissions: [])).Success;

        // Act
        var result = await client.GetRole(role.Id);

        // Assert
        var found = result.Success;
        found.Id.Should().Be(role.Id);
        found.Name.Should().Be("Admin");
        found.Description.Should().Be("Administrador");
        found.BaseType.Should().Be(UserType.Manager);
        found.Permissions.Should().BeEmpty();
    }

    #endregion
}
