namespace Estud.Tests.Integration;

public partial class IntegrationTests
{
    #region Authentication

    [Test]
    public async Task Classes_StartClass_Should_not_start_class_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.StartClass(1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Authorization

    [Test]
    public async Task Classes_StartClass_Should_not_start_class_when_user_has_no_permission()
    {
        // Arrange
        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.StartClass(1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    #endregion

    #region Validation errors

    [Test]
    public async Task Classes_StartClass_Should_not_start_class_when_class_not_found()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.StartClass(999999);

        // Assert
        result.ShouldBeError(ClassNotFound.I);
    }

    [Test]
    public async Task Classes_StartClass_Should_not_start_class_when_class_is_not_on_enrollment()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var discipline = (await client.CreateDiscipline()).Success;
        var period = (await client.CreateAcademicPeriod()).Success;
        var @class = (await client.CreateClass(discipline.Id, period.Id)).Success;

        // Act
        var result = await client.StartClass(@class.Id);

        // Assert
        result.ShouldBeError(ClassMustBeOnEnrollment.I);
    }

    [Test]
    public async Task Classes_StartClass_Should_not_start_class_when_enrollment_period_is_not_finalized()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var discipline = (await client.CreateDiscipline()).Success;
        var period = (await client.CreateAcademicPeriod()).Success;
        var @class = (await client.CreateClass(discipline.Id, period.Id)).Success;

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        await client.CreateEnrollmentPeriod(startAt: today.AddDays(-2), endAt: today.AddDays(2));
        await client.ReleaseClassForEnrollment(@class.Id);

        // Act
        var result = await client.StartClass(@class.Id);

        // Assert
        result.ShouldBeError(EnrollmentPeriodMustBeFinalized.I);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task Classes_StartClass_Should_start_class()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var discipline = (await client.CreateDiscipline()).Success;
        var period = (await client.CreateAcademicPeriod()).Success;
        var @class = (await client.CreateClass(discipline.Id, period.Id)).Success;

        await using (var ctx = _back.GetDbContext())
        {
            var entity = await ctx.Classes.FirstAsync(c => c.Id == @class.Id);
            entity.Status = ClassStatus.OnEnrollment;
            await ctx.SaveChangesAsync();
        }

        // Act
        var result = await client.StartClass(@class.Id);

        // Assert
        result.ShouldBeSuccess();

        await using var db = _back.GetDbContext();
        var started = await db.Classes.FirstAsync(c => c.Id == @class.Id);
        started.Status.Should().Be(ClassStatus.Started);
    }

    #endregion
}
