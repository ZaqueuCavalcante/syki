namespace Estud.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    #region Authentication

    [Test]
    public async Task Identity_GetTwoFactorEnforcement_Should_not_get_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.GetTwoFactorEnforcement();

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Authorization

    [Test]
    public async Task Identity_GetTwoFactorEnforcement_Should_not_get_without_permission()
    {
        // Arrange
        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.GetTwoFactorEnforcement();

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task Identity_GetTwoFactorEnforcement_Should_get_institution_roles()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.GetTwoFactorEnforcement().Success();

        // Assert
        result.Total.Should().BeGreaterThan(0);
        result.Items.Should().OnlyContain(r => !r.TwoFactorRequired);
        result.Items.Should().Contain(r => r.BaseType == UserType.Manager);
    }

    #endregion
}
