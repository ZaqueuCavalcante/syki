namespace Estud.Tests.Integration;

public partial class IntegrationTests
{
    #region Authentication

    [Test]
    public async Task Classes_UpdateClassSchedules_Should_not_update_schedules_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.UpdateClassSchedules(1, []);

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Authorization

    [Test]
    public async Task Classes_UpdateClassSchedules_Should_not_update_schedules_when_user_has_no_permission()
    {
        // Arrange
        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.UpdateClassSchedules(1, []);

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    #endregion

    #region Validation errors

    [Test]
    public async Task Classes_UpdateClassSchedules_Should_not_update_schedules_when_class_not_found()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.UpdateClassSchedules(999999, [(Day.Monday, Hour.H07_00, Hour.H10_00)]);

        // Assert
        result.ShouldBeError(ClassNotFound.I);
    }

    [Test]
    public async Task Classes_UpdateClassSchedules_Should_not_update_schedules_when_class_is_already_started()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var discipline = (await client.CreateDiscipline()).Success;
        var period = (await client.CreateAcademicPeriod()).Success;
        var @class = (await client.CreateClass(discipline.Id, period.Id)).Success;

        await client.StartClass(@class.Id);

        // Act
        var result = await client.UpdateClassSchedules(@class.Id, [(Day.Monday, Hour.H07_00, Hour.H10_00)]);

        // Assert
        result.ShouldBeError(ClassAlreadyStarted.I);
    }

    [Test]
    public async Task Classes_UpdateClassSchedules_Should_not_update_schedules_when_a_schedule_is_malformed()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var discipline = (await client.CreateDiscipline()).Success;
        var period = (await client.CreateAcademicPeriod()).Success;
        var @class = (await client.CreateClass(discipline.Id, period.Id)).Success;

        // Act
        var result = await client.UpdateClassSchedules(@class.Id, [(Day.Monday, Hour.H10_00, Hour.H07_00)]);

        // Assert
        result.ShouldBeError(InvalidSchedule.I);
    }

    [Test]
    public async Task Classes_UpdateClassSchedules_Should_not_update_schedules_when_schedules_conflict_with_each_other()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var discipline = (await client.CreateDiscipline()).Success;
        var period = (await client.CreateAcademicPeriod()).Success;
        var @class = (await client.CreateClass(discipline.Id, period.Id)).Success;

        // Act
        var result = await client.UpdateClassSchedules(@class.Id,
        [
            (Day.Monday, Hour.H07_00, Hour.H10_00),
            (Day.Monday, Hour.H08_00, Hour.H09_00),
        ]);

        // Assert
        result.ShouldBeError(ConflictingSchedules.I);
    }

    [Test]
    public async Task Classes_UpdateClassSchedules_Should_not_update_schedules_when_they_conflict_with_another_class_of_the_teacher()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var discipline = (await client.CreateDiscipline()).Success;
        var period = (await client.CreateAcademicPeriod()).Success;

        var teacher = (await client.CreateTeacher("Chico Ferreira", DataGen.Email)).Success;
        await client.AssignDisciplinesToTeacher(teacher.Id, [discipline.Id]);

        var classA = (await client.CreateClass(discipline.Id, period.Id)).Success;
        await client.AssignTeachersToClass(classA.Id, [teacher.Id]);
        await client.UpdateClassSchedules(classA.Id, [(Day.Monday, Hour.H07_00, Hour.H10_00)]);

        var classB = (await client.CreateClass(discipline.Id, period.Id)).Success;
        await client.AssignTeachersToClass(classB.Id, [teacher.Id]);

        // Act
        var result = await client.UpdateClassSchedules(classB.Id, [(Day.Monday, Hour.H08_00, Hour.H09_00)]);

        // Assert
        result.ShouldBeError(TeacherScheduleConflict.I);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task Classes_UpdateClassSchedules_Should_set_the_schedules_of_the_class()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var discipline = (await client.CreateDiscipline()).Success;
        var period = (await client.CreateAcademicPeriod()).Success;
        var @class = (await client.CreateClass(discipline.Id, period.Id)).Success;

        // Act
        var result = await client.UpdateClassSchedules(@class.Id,
        [
            (Day.Wednesday, Hour.H07_00, Hour.H10_00),
            (Day.Monday, Hour.H07_00, Hour.H10_00),
        ]);

        // Assert
        result.ShouldBeSuccess();

        var updated = (await client.GetClass(@class.Id)).Success;
        updated.Schedules.Select(s => s.Day).Should().Equal(Day.Monday, Day.Wednesday);
    }

    [Test]
    public async Task Classes_UpdateClassSchedules_Should_replace_the_current_schedules_of_the_class()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var discipline = (await client.CreateDiscipline()).Success;
        var period = (await client.CreateAcademicPeriod()).Success;
        var @class = (await client.CreateClass(discipline.Id, period.Id)).Success;
        await client.UpdateClassSchedules(@class.Id, [(Day.Monday, Hour.H07_00, Hour.H10_00)]);

        // Act
        var result = await client.UpdateClassSchedules(@class.Id, [(Day.Tuesday, Hour.H19_00, Hour.H22_00)]);

        // Assert
        result.ShouldBeSuccess();

        var updated = (await client.GetClass(@class.Id)).Success;
        updated.Schedules.Should().HaveCount(1);
        updated.Schedules[0].Day.Should().Be(Day.Tuesday);
        updated.Schedules[0].StartAt.Should().Be(Hour.H19_00);
        updated.Schedules[0].EndAt.Should().Be(Hour.H22_00);
    }

    [Test]
    public async Task Classes_UpdateClassSchedules_Should_remove_all_schedules_of_the_class_when_list_is_empty()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var discipline = (await client.CreateDiscipline()).Success;
        var period = (await client.CreateAcademicPeriod()).Success;
        var @class = (await client.CreateClass(discipline.Id, period.Id)).Success;
        await client.UpdateClassSchedules(@class.Id, [(Day.Monday, Hour.H07_00, Hour.H10_00)]);

        // Act
        var result = await client.UpdateClassSchedules(@class.Id, []);

        // Assert
        result.ShouldBeSuccess();

        var updated = (await client.GetClass(@class.Id)).Success;
        updated.Schedules.Should().BeEmpty();
    }

    [Test]
    public async Task Classes_UpdateClassSchedules_Should_allow_the_same_schedule_for_two_different_teachers()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var discipline = (await client.CreateDiscipline()).Success;
        var period = (await client.CreateAcademicPeriod()).Success;

        var chico = (await client.CreateTeacher("Chico Ferreira", DataGen.Email)).Success;
        var ana = (await client.CreateTeacher("Ana Lima", DataGen.Email)).Success;
        await client.AssignDisciplinesToTeacher(chico.Id, [discipline.Id]);
        await client.AssignDisciplinesToTeacher(ana.Id, [discipline.Id]);

        var classA = (await client.CreateClass(discipline.Id, period.Id)).Success;
        await client.AssignTeachersToClass(classA.Id, [chico.Id]);
        await client.UpdateClassSchedules(classA.Id, [(Day.Monday, Hour.H07_00, Hour.H10_00)]);

        var classB = (await client.CreateClass(discipline.Id, period.Id)).Success;
        await client.AssignTeachersToClass(classB.Id, [ana.Id]);

        // Act
        var result = await client.UpdateClassSchedules(classB.Id, [(Day.Monday, Hour.H07_00, Hour.H10_00)]);

        // Assert
        result.ShouldBeSuccess();
    }

    #endregion
}
