namespace Estud.Tests.Integration;

public partial class IntegrationTests
{
    #region Authentication

    [Test]
    public async Task Institutions_SetupInstitutionConfig_Should_not_setup_institution_config_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.SetupInstitutionConfig();

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Authorization

    [Test]
    public async Task Institutions_SetupInstitutionConfig_Should_not_setup_institution_config_when_user_has_no_permission()
    {
        // Arrange
        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.SetupInstitutionConfig();

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    #endregion

    #region Validation errors

    [Test]
    [TestCase(-0.01)]
    [TestCase(-1.00)]
    [TestCase(10.01)]
    [TestCase(11.00)]
    public async Task Institutions_SetupInstitutionConfig_Should_not_setup_institution_config_with_invalid_note_limit(decimal noteLimit)
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.SetupInstitutionConfig(noteLimit, 70.00M);

        // Assert
        result.ShouldBeError(InvalidNoteLimit.I);
    }

    [Test]
    [TestCase(-0.01)]
    [TestCase(-1.00)]
    [TestCase(100.01)]
    [TestCase(150.00)]
    public async Task Institutions_SetupInstitutionConfig_Should_not_setup_institution_config_with_invalid_frequency_limit(decimal frequencyLimit)
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.SetupInstitutionConfig(7.00M, frequencyLimit);

        // Assert
        result.ShouldBeError(InvalidFrequencyLimit.I);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task Institutions_SetupInstitutionConfig_Should_create_institution_config_with_default_values_on_institution_creation()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var institutionId = client.User.InstitutionId;

        // Act
        await using var ctx = _back.GetDbContext();
        var config = await ctx.InstitutionConfigs.FirstOrDefaultAsync(x => x.InstitutionId == institutionId);

        // Assert
        config.Should().NotBeNull();
        config!.NoteLimit.Should().Be(7.00M);
        config.FrequencyLimit.Should().Be(70.00M);
    }

    [Test]
    public async Task Institutions_SetupInstitutionConfig_Should_setup_institution_config()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var institutionId = client.User.InstitutionId;

        // Act
        var result = await client.SetupInstitutionConfig(8.50M, 85.00M);

        // Assert
        var config = result.Success;
        config.Id.Should().NotBe(0);
        config.NoteLimit.Should().Be(8.50M);
        config.FrequencyLimit.Should().Be(85.00M);

        await using var ctx = _back.GetDbContext();
        var saved = await ctx.InstitutionConfigs.FirstAsync(x => x.InstitutionId == institutionId);
        saved.Id.Should().Be(config.Id);
        saved.NoteLimit.Should().Be(8.50M);
        saved.FrequencyLimit.Should().Be(85.00M);
    }

    [Test]
    public async Task Institutions_SetupInstitutionConfig_Should_setup_institution_config_with_limit_values()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.SetupInstitutionConfig(0.00M, 0.00M);

        // Assert
        var config = result.Success;
        config.NoteLimit.Should().Be(0.00M);
        config.FrequencyLimit.Should().Be(0.00M);

        // Act
        var otherResult = await client.SetupInstitutionConfig(10.00M, 100.00M);

        // Assert
        var otherConfig = otherResult.Success;
        otherConfig.Id.Should().Be(config.Id);
        otherConfig.NoteLimit.Should().Be(10.00M);
        otherConfig.FrequencyLimit.Should().Be(100.00M);
    }

    #endregion
}
