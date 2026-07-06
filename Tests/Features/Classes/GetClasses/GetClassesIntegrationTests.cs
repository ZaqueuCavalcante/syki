namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    #region Authentication

    [Test]
    public async Task Classes_GetClasses_Should_not_get_classes_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.GetClasses();

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Authorization

    [Test]
    public async Task Classes_GetClasses_Should_not_get_classes_when_user_has_no_permission()
    {
        // Arrange
        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.GetClasses();

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task Classes_GetClasses_Should_return_empty_list()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.GetClasses();

        // Assert
        var classes = result.Success;
        classes.Total.Should().Be(0);
        classes.Items.Should().BeEmpty();
    }

    [Test]
    public async Task Classes_GetClasses_Should_return_classes()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var discipline = (await client.CreateDiscipline("Banco de Dados")).Success;
        var period = (await client.CreateAcademicPeriod("2024.1")).Success;
        await client.CreateClass(discipline.Id, period.Id);

        // Act
        var result = await client.GetClasses();

        // Assert
        var classes = result.Success;
        classes.Total.Should().Be(1);
        classes.Items[0].Discipline.Should().Be("Banco de Dados");
        classes.Items[0].Period.Should().Be("2024.1");
        classes.Items[0].Status.Should().Be(ClassStatus.OnPreEnrollment);
        classes.Items[0].Schedules.Should().HaveCount(1);
    }

    [Test]
    public async Task Classes_GetClasses_Should_return_classes_filtered_by_status()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var discipline = (await client.CreateDiscipline()).Success;
        var period = (await client.CreateAcademicPeriod("2024.1")).Success;
        await client.CreateClass(discipline.Id, period.Id);

        // Act
        var onPreEnrollment = (await client.GetClasses(ClassStatus.OnPreEnrollment)).Success;
        var finalized = (await client.GetClasses(ClassStatus.Finalized)).Success;

        // Assert
        onPreEnrollment.Total.Should().Be(1);
        finalized.Total.Should().Be(0);
    }

    #endregion
}
