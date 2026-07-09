namespace Estud.Tests.Integration;

public partial class IntegrationTests
{
    #region Authentication

    [Test]
    public async Task Identity_GetPermissions_Should_not_get_permissions_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.GetPermissions();

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Authorization

    [Test]
    public async Task Identity_GetPermissions_Should_not_get_permissions_when_user_has_no_permission()
    {
        // Arrange
        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.GetPermissions();

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task Identity_GetPermissions_Should_get_all_permissions()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.GetPermissions();

        // Assert
        var permissions = result.Success;
        permissions.Items.Should().HaveCount(EstudPermissions.Permissions.Count);
        permissions.Items.Should().Contain(x => x.Id == EstudPermissions.ManageRoles.Id);
        permissions.Items.Should().OnlyContain(x => x.AllowedTypes.Count > 0);
    }

    #endregion
}
