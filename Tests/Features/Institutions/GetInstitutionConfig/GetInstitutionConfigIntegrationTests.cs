namespace Estud.Tests.Integration;

public partial class IntegrationTests
{
    #region Authentication

    [Test]
    public async Task Institutions_GetInstitutionConfig_Should_not_get_institution_config_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.GetInstitutionConfig();

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Authorization

    [Test]
    public async Task Institutions_GetInstitutionConfig_Should_not_get_institution_config_when_user_has_no_permission()
    {
        // Arrange
        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.GetInstitutionConfig();

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task Institutions_GetInstitutionConfig_Should_get_institution_config_with_default_values()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.GetInstitutionConfig();

        // Assert
        var config = result.Success;
        config.Id.Should().NotBe(0);
        config.NoteLimit.Should().Be(7.00M);
        config.FrequencyLimit.Should().Be(70.00M);
    }

    [Test]
    public async Task Institutions_GetInstitutionConfig_Should_get_institution_config_after_setup()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        await client.SetupInstitutionConfig(6.00M, 75.00M);

        // Act
        var result = await client.GetInstitutionConfig();

        // Assert
        var config = result.Success;
        config.NoteLimit.Should().Be(6.00M);
        config.FrequencyLimit.Should().Be(75.00M);
    }

    #endregion
}
