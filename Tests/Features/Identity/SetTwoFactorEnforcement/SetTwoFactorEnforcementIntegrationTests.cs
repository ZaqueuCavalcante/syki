namespace Estud.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    #region Authentication

    [Test]
    public async Task Identity_SetTwoFactorEnforcement_Should_not_set_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.SetTwoFactorEnforcement(1, true);

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Authorization

    [Test]
    public async Task Identity_SetTwoFactorEnforcement_Should_not_set_without_permission()
    {
        // Arrange
        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.SetTwoFactorEnforcement(1, true);

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    #endregion

    #region Validation errors

    [Test]
    public async Task Identity_SetTwoFactorEnforcement_Should_not_set_for_nonexistent_role()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.SetTwoFactorEnforcement(999999, true);

        // Assert
        result.ShouldBeError(RoleNotFound.I);
    }

    [Test]
    public async Task Identity_SetTwoFactorEnforcement_Should_not_set_for_role_from_another_institution()
    {
        // Arrange - cada director registra uma nova instituição; pegamos uma role real da instituição B
        var directorB = await _back.LoggedAsDirector();
        var rolesB = await directorB.GetRoles().Success();
        var roleFromB = rolesB.Items.First(r => r.BaseType == UserType.Manager);

        var directorA = await _back.LoggedAsDirector();

        // Act - director A tenta alterar o enforcement de uma role da instituição B
        var result = await directorA.SetTwoFactorEnforcement(roleFromB.Id, true);

        // Assert - a role existe, mas não pertence à instituição de A → RoleNotFound (isolamento multi-tenant)
        result.ShouldBeError(RoleNotFound.I);

        // Controle: a role da instituição B continua sem enforcement
        var afterB = await directorB.GetTwoFactorEnforcement().Success();
        afterB.Items.First(r => r.RoleId == roleFromB.Id).TwoFactorRequired.Should().BeFalse();
    }

    #endregion

    #region Happy path

    [Test]
    public async Task Identity_SetTwoFactorEnforcement_Should_toggle_the_flag()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var roles = await client.GetRoles().Success();
        var role = roles.Items.First(r => r.BaseType == UserType.Manager);

        // Act
        var enabled = await client.SetTwoFactorEnforcement(role.Id, true).Success();

        // Assert
        enabled.RoleId.Should().Be(role.Id);
        enabled.TwoFactorRequired.Should().BeTrue();

        var afterEnable = await client.GetTwoFactorEnforcement().Success();
        afterEnable.Items.First(r => r.RoleId == role.Id).TwoFactorRequired.Should().BeTrue();

        // Act - desliga de novo
        var disabled = await client.SetTwoFactorEnforcement(role.Id, false).Success();

        // Assert
        disabled.TwoFactorRequired.Should().BeFalse();
        var afterDisable = await client.GetTwoFactorEnforcement().Success();
        afterDisable.Items.First(r => r.RoleId == role.Id).TwoFactorRequired.Should().BeFalse();
    }

    #endregion
}
