namespace Estud.Tests.Integration;

public partial class IntegrationTests
{
    #region Authentication

    [Test]
    public async Task Classes_UpdateClassClassrooms_Should_not_update_classrooms_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.UpdateClassClassrooms(classId: 1, classrooms: []);

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Authorization

    [Test]
    public async Task Classes_UpdateClassClassrooms_Should_not_update_classrooms_when_user_has_no_permission()
    {
        // Arrange
        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.UpdateClassClassrooms(classId: 1, classrooms: []);

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    #endregion

    #region Validation errors

    [Test]
    public async Task Classes_UpdateClassClassrooms_Should_not_update_classrooms_when_class_not_found()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.UpdateClassClassrooms(classId: 999999, classrooms: []);

        // Assert
        result.ShouldBeError(ClassNotFound.I);
    }

    [Test]
    public async Task Classes_UpdateClassClassrooms_Should_not_update_classrooms_when_class_is_already_started()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var campus = await client.CreateCampus().Success();
        var discipline = await client.CreateDiscipline().Success();
        var period = await client.CreateAcademicPeriod().Success();
        var @class = await client.CreateClass(discipline.Id, period.Id, campusId: campus.Id).Success();

        var teacher = await client.CreateTeacher(DataGen.UserName, DataGen.Email).Success();
        await client.AssignDisciplinesToTeacher(teacher.Id, [discipline.Id]);
        await client.UpdateClassTeachers(@class.Id, [teacher.Id]);
        await client.UpdateClassSchedules(@class.Id, [(Day.Monday, Hour.H07_00, Hour.H10_00, null)]);

        await client.ReleaseClassForEnrollment(@class.Id);
        await client.StartClass(@class.Id);

        var classroom = await client.CreateClassroom(campus.Id).Success();

        // Act
        var result = await client.UpdateClassClassrooms(@class.Id, classrooms: [(Day.Monday, Hour.H07_00, Hour.H10_00, classroom.Id)]);

        // Assert
        result.ShouldBeError(ClassAlreadyStarted.I);
    }

    [Test]
    public async Task Classes_UpdateClassClassrooms_Should_not_update_classrooms_when_class_is_finalized()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var campus = await client.CreateCampus().Success();
        var classroom = await client.CreateClassroom(campus.Id).Success();
        var discipline = await client.CreateDiscipline().Success();
        var period = await client.CreateAcademicPeriod().Success();
        var @class = await client.CreateClass(discipline.Id, period.Id, campusId: campus.Id).Success();
        await client.UpdateClassSchedules(@class.Id, [(Day.Monday, Hour.H07_00, Hour.H10_00, null)]);

        await using (var ctx = _back.GetDbContext())
        {
            var entity = await ctx.Classes.FirstAsync(c => c.Id == @class.Id);
            entity.Status = ClassStatus.Finalized;
            await ctx.SaveChangesAsync();
        }

        // Act
        var result = await client.UpdateClassClassrooms(@class.Id, [(Day.Monday, Hour.H07_00, Hour.H10_00, classroom.Id)]);

        // Assert
        result.ShouldBeError(ClassAlreadyStarted.I);
    }

    [Test]
    public async Task Classes_UpdateClassClassrooms_Should_not_update_classrooms_when_schedule_is_not_in_the_class()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var campus = await client.CreateCampus().Success();
        var classroom = await client.CreateClassroom(campus.Id).Success();
        var discipline = await client.CreateDiscipline().Success();
        var period = await client.CreateAcademicPeriod().Success();
        var @class = await client.CreateClass(discipline.Id, period.Id, campusId: campus.Id).Success();
        await client.UpdateClassSchedules(@class.Id, [(Day.Monday, Hour.H07_00, Hour.H10_00, null)]);

        // Act — horário informado (terça) não existe na turma
        var result = await client.UpdateClassClassrooms(@class.Id, [(Day.Tuesday, Hour.H07_00, Hour.H10_00, classroom.Id)]);

        // Assert
        result.ShouldBeError(ScheduleNotFound.I);
    }

    [Test]
    public async Task Classes_UpdateClassClassrooms_Should_not_update_classrooms_when_classroom_not_found()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var campus = await client.CreateCampus().Success();
        var discipline = await client.CreateDiscipline().Success();
        var period = await client.CreateAcademicPeriod().Success();
        var @class = await client.CreateClass(discipline.Id, period.Id, campusId: campus.Id).Success();
        await client.UpdateClassSchedules(@class.Id, [(Day.Monday, Hour.H07_00, Hour.H10_00, null)]);

        // Act
        var result = await client.UpdateClassClassrooms(@class.Id, [(Day.Monday, Hour.H07_00, Hour.H10_00, 999999)]);

        // Assert
        result.ShouldBeError(ClassroomNotFound.I);
    }

    [Test]
    public async Task Classes_UpdateClassClassrooms_Should_not_update_classrooms_when_classroom_is_on_another_campus()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var campusA = await client.CreateCampus("Agreste I").Success();
        var campusB = await client.CreateCampus("Agreste II").Success();
        var classroom = await client.CreateClassroom(campusB.Id).Success();
        var discipline = await client.CreateDiscipline().Success();
        var period = await client.CreateAcademicPeriod().Success();
        var @class = await client.CreateClass(discipline.Id, period.Id, campusId: campusA.Id).Success();
        await client.UpdateClassSchedules(@class.Id, [(Day.Monday, Hour.H07_00, Hour.H10_00, null)]);

        // Act
        var result = await client.UpdateClassClassrooms(@class.Id, [(Day.Monday, Hour.H07_00, Hour.H10_00, classroom.Id)]);

        // Assert
        result.ShouldBeError(ClassAndClassroomDifferentCampus.I);
    }

    [Test]
    public async Task Classes_UpdateClassClassrooms_Should_not_update_classrooms_when_classroom_capacity_is_smaller_than_class_vacancies()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var campus = await client.CreateCampus().Success();
        var classroom = await client.CreateClassroom(campus.Id, capacity: 30).Success();
        var discipline = await client.CreateDiscipline().Success();
        var period = await client.CreateAcademicPeriod().Success();
        var @class = await client.CreateClass(discipline.Id, period.Id, vacancies: 40, campusId: campus.Id).Success();
        await client.UpdateClassSchedules(@class.Id, [(Day.Monday, Hour.H07_00, Hour.H10_00, null)]);

        // Act
        var result = await client.UpdateClassClassrooms(@class.Id, [(Day.Monday, Hour.H07_00, Hour.H10_00, classroom.Id)]);

        // Assert
        result.ShouldBeError(ClassroomCapacityExceeded.I);
    }

    [Test]
    public async Task Classes_UpdateClassClassrooms_Should_not_update_classrooms_when_they_conflict_with_another_class_in_the_same_classroom()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var campus = await client.CreateCampus().Success();
        var classroom = await client.CreateClassroom(campus.Id).Success();
        var discipline = await client.CreateDiscipline().Success();
        var period = await client.CreateAcademicPeriod().Success();

        var classA = await client.CreateClass(discipline.Id, period.Id, campusId: campus.Id).Success();
        await client.UpdateClassSchedules(classA.Id, [(Day.Monday, Hour.H07_00, Hour.H10_00, null)]);
        await client.UpdateClassClassrooms(classA.Id, [(Day.Monday, Hour.H07_00, Hour.H10_00, classroom.Id)]);

        var classB = await client.CreateClass(discipline.Id, period.Id, campusId: campus.Id).Success();
        await client.UpdateClassSchedules(classB.Id, [(Day.Monday, Hour.H08_00, Hour.H09_00, null)]);

        // Act — mesma sala, horário sobreposto
        var result = await client.UpdateClassClassrooms(classB.Id, [(Day.Monday, Hour.H08_00, Hour.H09_00, classroom.Id)]);

        // Assert
        result.ShouldBeError(ClassroomScheduleConflict.I);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task Classes_UpdateClassClassrooms_Should_assign_a_classroom_to_the_class_schedule()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var campus = await client.CreateCampus().Success();
        var classroom = await client.CreateClassroom(campus.Id).Success();
        var discipline = await client.CreateDiscipline().Success();
        var period = await client.CreateAcademicPeriod().Success();
        var @class = await client.CreateClass(discipline.Id, period.Id, campusId: campus.Id).Success();
        await client.UpdateClassSchedules(@class.Id, [(Day.Monday, Hour.H07_00, Hour.H10_00, null)]);

        // Act
        var result = await client.UpdateClassClassrooms(@class.Id, [(Day.Monday, Hour.H07_00, Hour.H10_00, classroom.Id)]);

        // Assert
        result.ShouldBeSuccess();

        var updated = await client.GetClassroom(classroom.Id).Success();
        updated.Schedules.Should().ContainSingle();
        updated.Schedules[0].ClassId.Should().Be(@class.Id);
        updated.Schedules[0].Day.Should().Be(Day.Monday);
        updated.Schedules[0].StartAt.Should().Be(Hour.H07_00);
        updated.Schedules[0].EndAt.Should().Be(Hour.H10_00);
    }

    [Test]
    public async Task Classes_UpdateClassClassrooms_Should_expose_the_classroom_on_the_class_schedule()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var campus = await client.CreateCampus().Success();
        var classroom = await client.CreateClassroom(campus.Id, "Sala 07").Success();
        var discipline = await client.CreateDiscipline().Success();
        var period = await client.CreateAcademicPeriod().Success();
        var @class = await client.CreateClass(discipline.Id, period.Id, campusId: campus.Id).Success();
        await client.UpdateClassSchedules(@class.Id, [(Day.Monday, Hour.H07_00, Hour.H10_00, null)]);

        // Act
        var result = await client.UpdateClassClassrooms(@class.Id, [(Day.Monday, Hour.H07_00, Hour.H10_00, classroom.Id)]);

        // Assert
        result.ShouldBeSuccess();

        var updated = await client.GetClass(@class.Id).Success();
        updated.CampusId.Should().Be(campus.Id);
        updated.Schedules.Should().ContainSingle();
        updated.Schedules[0].ClassroomId.Should().Be(classroom.Id);
        updated.Schedules[0].Classroom.Should().Be("Sala 07");
    }

    [Test]
    public async Task Classes_UpdateClassClassrooms_Should_assign_a_classroom_per_schedule()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var campus = await client.CreateCampus().Success();
        var classroomA = await client.CreateClassroom(campus.Id, "Sala 05").Success();
        var classroomB = await client.CreateClassroom(campus.Id, "Sala 06").Success();
        var discipline = await client.CreateDiscipline().Success();
        var period = await client.CreateAcademicPeriod().Success();
        var @class = await client.CreateClass(discipline.Id, period.Id, campusId: campus.Id).Success();
        await client.UpdateClassSchedules(@class.Id,
        [
            (Day.Monday, Hour.H07_00, Hour.H10_00, null),
            (Day.Wednesday, Hour.H07_00, Hour.H10_00, null),
        ]);

        // Act — segunda na sala A, quarta na sala B
        var result = await client.UpdateClassClassrooms(@class.Id,
        [
            (Day.Monday, Hour.H07_00, Hour.H10_00, classroomA.Id),
            (Day.Wednesday, Hour.H07_00, Hour.H10_00, classroomB.Id),
        ]);

        // Assert
        result.ShouldBeSuccess();

        var updatedA = await client.GetClassroom(classroomA.Id).Success();
        updatedA.Schedules.Should().ContainSingle();
        updatedA.Schedules[0].Day.Should().Be(Day.Monday);

        var updatedB = await client.GetClassroom(classroomB.Id).Success();
        updatedB.Schedules.Should().ContainSingle();
        updatedB.Schedules[0].Day.Should().Be(Day.Wednesday);
    }

    [Test]
    public async Task Classes_UpdateClassClassrooms_Should_move_the_schedule_to_another_classroom()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var campus = await client.CreateCampus().Success();
        var classroomA = await client.CreateClassroom(campus.Id, "Sala 05").Success();
        var classroomB = await client.CreateClassroom(campus.Id, "Sala 06").Success();
        var discipline = await client.CreateDiscipline().Success();
        var period = await client.CreateAcademicPeriod().Success();
        var @class = await client.CreateClass(discipline.Id, period.Id, campusId: campus.Id).Success();
        await client.UpdateClassSchedules(@class.Id, [(Day.Monday, Hour.H07_00, Hour.H10_00, null)]);
        await client.UpdateClassClassrooms(@class.Id, [(Day.Monday, Hour.H07_00, Hour.H10_00, classroomA.Id)]);

        // Act — realoca para a sala B
        var result = await client.UpdateClassClassrooms(@class.Id, [(Day.Monday, Hour.H07_00, Hour.H10_00, classroomB.Id)]);

        // Assert
        result.ShouldBeSuccess();

        var updatedA = await client.GetClassroom(classroomA.Id).Success();
        updatedA.Schedules.Should().BeEmpty();

        var updatedB = await client.GetClassroom(classroomB.Id).Success();
        updatedB.Schedules.Should().ContainSingle();
        updatedB.Schedules[0].ClassId.Should().Be(@class.Id);
    }

    [Test]
    public async Task Classes_UpdateClassClassrooms_Should_clear_all_classrooms_when_list_is_empty()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var campus = await client.CreateCampus().Success();
        var classroom = await client.CreateClassroom(campus.Id).Success();
        var discipline = await client.CreateDiscipline().Success();
        var period = await client.CreateAcademicPeriod().Success();
        var @class = await client.CreateClass(discipline.Id, period.Id, campusId: campus.Id).Success();
        await client.UpdateClassSchedules(@class.Id, [(Day.Monday, Hour.H07_00, Hour.H10_00, null)]);
        await client.UpdateClassClassrooms(@class.Id, [(Day.Monday, Hour.H07_00, Hour.H10_00, classroom.Id)]);

        // Act
        var result = await client.UpdateClassClassrooms(@class.Id, []);

        // Assert
        result.ShouldBeSuccess();

        var updated = await client.GetClassroom(classroom.Id).Success();
        updated.Schedules.Should().BeEmpty();
    }

    [Test]
    public async Task Classes_UpdateClassClassrooms_Should_share_a_classroom_between_classes_on_different_days()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var campus = await client.CreateCampus().Success();
        var classroom = await client.CreateClassroom(campus.Id).Success();
        var discipline = await client.CreateDiscipline().Success();
        var period = await client.CreateAcademicPeriod().Success();

        var classA = await client.CreateClass(discipline.Id, period.Id, campusId: campus.Id).Success();
        await client.UpdateClassSchedules(classA.Id, [(Day.Monday, Hour.H07_00, Hour.H10_00, null)]);
        await client.UpdateClassClassrooms(classA.Id, [(Day.Monday, Hour.H07_00, Hour.H10_00, classroom.Id)]);

        var classB = await client.CreateClass(discipline.Id, period.Id, campusId: campus.Id).Success();
        await client.UpdateClassSchedules(classB.Id, [(Day.Wednesday, Hour.H07_00, Hour.H10_00, null)]);

        // Act — mesma sala, mas na quarta (livre)
        var result = await client.UpdateClassClassrooms(classB.Id, [(Day.Wednesday, Hour.H07_00, Hour.H10_00, classroom.Id)]);

        // Assert
        result.ShouldBeSuccess();

        var updated = await client.GetClassroom(classroom.Id).Success();
        updated.Schedules.Should().HaveCount(2);
    }

    [Test]
    public async Task Classes_UpdateClassClassrooms_Should_not_conflict_with_a_finalized_class_in_the_same_classroom()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var campus = await client.CreateCampus().Success();
        var classroom = await client.CreateClassroom(campus.Id).Success();
        var discipline = await client.CreateDiscipline().Success();
        var period = await client.CreateAcademicPeriod().Success();

        var classA = await client.CreateClass(discipline.Id, period.Id, campusId: campus.Id).Success();
        await client.UpdateClassSchedules(classA.Id, [(Day.Monday, Hour.H07_00, Hour.H10_00, null)]);
        await client.UpdateClassClassrooms(classA.Id, [(Day.Monday, Hour.H07_00, Hour.H10_00, classroom.Id)]);

        await using (var ctx = _back.GetDbContext())
        {
            var entity = await ctx.Classes.FirstAsync(c => c.Id == classA.Id);
            entity.Status = ClassStatus.Finalized;
            await ctx.SaveChangesAsync();
        }

        var classB = await client.CreateClass(discipline.Id, period.Id, campusId: campus.Id).Success();
        await client.UpdateClassSchedules(classB.Id, [(Day.Monday, Hour.H07_00, Hour.H10_00, null)]);

        // Act — mesmo horário da turma finalizada
        var result = await client.UpdateClassClassrooms(classB.Id, [(Day.Monday, Hour.H07_00, Hour.H10_00, classroom.Id)]);

        // Assert
        result.ShouldBeSuccess();
    }

    #endregion
}
