using Syki.Back.Features.Classes.CreateClass;

namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    #region Authentication

    [Test]
    public async Task Classes_CreateClass_Should_not_create_class_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.CreateClass(1, 1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Authorization

    [Test]
    public async Task Classes_CreateClass_Should_not_create_class_when_user_has_no_permission()
    {
        // Arrange
        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.CreateClass(1, 1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    #endregion

    #region Validation errors

    [Test]
    public async Task Classes_CreateClass_Should_not_create_class_when_campus_not_found()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var discipline = (await client.CreateDiscipline()).Success;
        var period = (await client.CreateAcademicPeriod()).Success;

        // Act
        var result = await client.CreateClass(discipline.Id, period.Id, campusId: 999999);

        // Assert
        result.ShouldBeError(CampusNotFound.I);
    }

    [Test]
    public async Task Classes_CreateClass_Should_not_create_class_when_discipline_not_found()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var period = (await client.CreateAcademicPeriod()).Success;

        // Act
        var result = await client.CreateClass(999999, period.Id);

        // Assert
        result.ShouldBeError(DisciplineNotFound.I);
    }

    [Test]
    public async Task Classes_CreateClass_Should_not_create_class_when_teacher_not_found()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var discipline = (await client.CreateDiscipline()).Success;
        var period = (await client.CreateAcademicPeriod()).Success;

        // Act
        var result = await client.CreateClass(discipline.Id, period.Id, teacherId: 999999);

        // Assert
        result.ShouldBeError(TeacherNotFound.I);
    }

    [Test]
    public async Task Classes_CreateClass_Should_not_create_class_when_teacher_not_assigned_to_campus()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var discipline = (await client.CreateDiscipline()).Success;
        var campus = (await client.CreateCampus()).Success;
        var teacher = (await client.CreateTeacher(DataGen.UserName, DataGen.Email)).Success;
        var period = (await client.CreateAcademicPeriod()).Success;

        // Act
        var result = await client.CreateClass(discipline.Id, period.Id, campusId: campus.Id, teacherId: teacher.Id);

        // Assert
        result.ShouldBeError(TeacherNotAssignedToCampus.I);
    }

    [Test]
    public async Task Classes_CreateClass_Should_not_create_class_when_teacher_not_assigned_to_discipline()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var discipline = (await client.CreateDiscipline()).Success;
        var teacher = (await client.CreateTeacher(DataGen.UserName, DataGen.Email)).Success;
        var period = (await client.CreateAcademicPeriod()).Success;

        // Act
        var result = await client.CreateClass(discipline.Id, period.Id, teacherId: teacher.Id);

        // Assert
        result.ShouldBeError(TeacherNotAssignedToDiscipline.I);
    }

    [Test]
    public async Task Classes_CreateClass_Should_not_create_class_when_academic_period_not_found()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var discipline = (await client.CreateDiscipline()).Success;

        // Act
        var result = await client.CreateClass(discipline.Id, 999999);

        // Assert
        result.ShouldBeError(AcademicPeriodNotFound.I);
    }

    [Test]
    public async Task Classes_CreateClass_Should_not_create_class_with_invalid_schedule()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var discipline = (await client.CreateDiscipline()).Success;
        var period = (await client.CreateAcademicPeriod()).Success;
        List<CreateClassScheduleIn> schedules = [new() { Day = Day.Monday, Start = Hour.H09_00, End = Hour.H07_00 }];

        // Act
        var result = await client.CreateClass(discipline.Id, period.Id, schedules: schedules);

        // Assert
        result.ShouldBeError(InvalidSchedule.I);
    }

    [Test]
    public async Task Classes_CreateClass_Should_not_create_class_with_conflicting_schedules()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var discipline = (await client.CreateDiscipline()).Success;
        var period = (await client.CreateAcademicPeriod()).Success;
        List<CreateClassScheduleIn> schedules =
        [
            new() { Day = Day.Monday, Start = Hour.H07_00, End = Hour.H09_00 },
            new() { Day = Day.Monday, Start = Hour.H08_00, End = Hour.H10_00 },
        ];

        // Act
        var result = await client.CreateClass(discipline.Id, period.Id, schedules: schedules);

        // Assert
        result.ShouldBeError(ConflictingSchedules.I);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task Classes_CreateClass_Should_create_class()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var discipline = (await client.CreateDiscipline()).Success;
        var period = (await client.CreateAcademicPeriod()).Success;

        // Act
        var result = await client.CreateClass(discipline.Id, period.Id);

        // Assert
        var @class = result.Success;
        @class.Id.Should().BeGreaterThan(0);
    }

    [Test]
    public async Task Classes_CreateClass_Should_create_class_with_campus_and_teacher()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var discipline = (await client.CreateDiscipline()).Success;
        var campus = (await client.CreateCampus()).Success;
        var teacher = (await client.CreateTeacher(DataGen.UserName, DataGen.Email)).Success;
        var period = (await client.CreateAcademicPeriod()).Success;
        await client.AssignCampiToTeacher(teacher.Id, [campus.Id]);
        await client.AssignDisciplinesToTeacher(teacher.Id, [discipline.Id]);

        // Act
        var result = await client.CreateClass(discipline.Id, period.Id, campusId: campus.Id, teacherId: teacher.Id);

        // Assert
        var @class = result.Success;
        @class.Id.Should().BeGreaterThan(0);
    }

    #endregion
}
