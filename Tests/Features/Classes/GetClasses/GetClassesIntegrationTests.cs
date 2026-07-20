namespace Estud.Tests.Integration;

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
        var discipline = await client.CreateDiscipline("Banco de Dados").Success();
        var period = await client.CreateAcademicPeriod("2024.1").Success();
        await client.CreateClass(discipline.Id, period.Id);

        // Act
        var result = await client.GetClasses();

        // Assert
        var classes = result.Success;
        classes.Total.Should().Be(1);
        classes.Page.Should().Be(1);
        classes.PageSize.Should().Be(10);
        classes.Items[0].Discipline.Should().Be("Banco de Dados");
        classes.Items[0].Period.Should().Be("2024.1");
        classes.Items[0].Status.Should().Be(ClassStatus.OnPreEnrollment);
        classes.Items[0].Schedules.Should().BeEmpty();
    }

    [Test]
    public async Task Classes_GetClasses_Should_return_class_as_on_enrollment_when_enrollment_period_is_open()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var discipline = await client.CreateDiscipline().Success();
        var period = await client.CreateAcademicPeriod().Success();
        var @class = await client.CreateClass(discipline.Id, period.Id).Success();

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        await client.CreateEnrollmentPeriod(startAt: today.AddDays(-2), endAt: today.AddDays(2));
        await client.ReleaseClassForEnrollment(@class.Id);

        // Act
        var result = await client.GetClasses();

        // Assert
        result.Success.Items[0].Status.Should().Be(ClassStatus.OnEnrollment);
    }

    [Test]
    public async Task Classes_GetClasses_Should_return_class_as_awaiting_start_when_enrollment_period_is_finalized()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var discipline = await client.CreateDiscipline().Success();
        var period = await client.CreateAcademicPeriod().Success();
        var @class = await client.CreateClass(discipline.Id, period.Id).Success();

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var enrollmentPeriod = await client.CreateEnrollmentPeriod(startAt: today.AddDays(-2), endAt: today.AddDays(2)).Success();
        await client.ReleaseClassForEnrollment(@class.Id);

        await client.UpdateEnrollmentPeriod(enrollmentPeriod.Id, startAt: today.AddDays(-2), endAt: today.AddDays(-1));

        // Act
        var result = await client.GetClasses();

        // Assert
        var item = result.Success.Items.First(x => x.Id == @class.Id);
        item.Status.Should().Be(ClassStatus.AwaitingStart);

        // And the persisted status is still OnEnrollment
        await using var db = _back.GetDbContext();
        var entity = await db.Classes.FirstAsync(c => c.Id == @class.Id);
        entity.Status.Should().Be(ClassStatus.OnEnrollment);
    }

    [Test]
    public async Task Classes_GetClasses_Should_return_classes_filtered_by_status()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var discipline = await client.CreateDiscipline().Success();
        var period = await client.CreateAcademicPeriod("2024.1").Success();
        await client.CreateClass(discipline.Id, period.Id);

        // Act
        var onPreEnrollment = await client.GetClasses(ClassStatus.OnPreEnrollment).Success();
        var finalized = await client.GetClasses(ClassStatus.Finalized).Success();

        // Assert
        onPreEnrollment.Total.Should().Be(1);
        finalized.Total.Should().Be(0);
    }

    [Test]
    public async Task Classes_GetClasses_Should_return_only_the_first_10_classes_by_default()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var period = await client.CreateAcademicPeriod("2024.1").Success();

        for (var i = 1; i <= 12; i++)
        {
            var discipline = await client.CreateDiscipline($"Disciplina {i:00}").Success();
            await client.CreateClass(discipline.Id, period.Id);
        }

        // Act
        var result = await client.GetClasses();

        // Assert
        var classes = result.Success;
        classes.Total.Should().Be(12);
        classes.Page.Should().Be(1);
        classes.PageSize.Should().Be(10);
        classes.Items.Should().HaveCount(10);
        classes.Items.First().Discipline.Should().Be("Disciplina 01");
        classes.Items.Last().Discipline.Should().Be("Disciplina 10");
    }

    [Test]
    public async Task Classes_GetClasses_Should_return_classes_from_the_second_page()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var period = await client.CreateAcademicPeriod("2024.1").Success();

        for (var i = 1; i <= 12; i++)
        {
            var discipline = await client.CreateDiscipline($"Disciplina {i:00}").Success();
            await client.CreateClass(discipline.Id, period.Id);
        }

        // Act
        var result = await client.GetClasses(page: 2);

        // Assert
        var classes = result.Success;
        classes.Total.Should().Be(12);
        classes.Page.Should().Be(2);
        classes.Items.Should().HaveCount(2);
        classes.Items.First().Discipline.Should().Be("Disciplina 11");
        classes.Items.Last().Discipline.Should().Be("Disciplina 12");
    }

    #endregion
}
