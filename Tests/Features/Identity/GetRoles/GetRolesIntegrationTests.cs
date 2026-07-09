namespace Estud.Tests.Integration;

public partial class IntegrationTests
{
    #region Authentication

    [Test]
    public async Task Identity_GetRoles_Should_not_get_roles_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.GetRoles();

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Authorization

    [Test]
    public async Task Identity_GetRoles_Should_not_get_roles_when_user_has_no_permission()
    {
        // Arrange
        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.GetRoles();

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task Identity_GetRoles_Should_get_roles()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        await client.CreateRole(name: "Admin", description: "Administrador");
        await client.CreateRole(name: "Coordenador", description: "Coordenador de curso");

        // Act
        var result = await client.GetRoles();

        // Assert
        var roles = result.Success;
        roles.Total.Should().Be(2);
        roles.Items.First().Name.Should().Be("Admin");
        roles.Items.Last().Name.Should().Be("Coordenador");
    }

    #endregion
}
