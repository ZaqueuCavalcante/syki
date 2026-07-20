namespace Estud.Tests.Integration;

public partial class IntegrationTests
{
    #region Authentication

    [Test]
    public async Task Classrooms_GetClassroom_Should_not_get_classroom_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.GetClassroom(id: 1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Authorization

    [Test]
    public async Task Classrooms_GetClassroom_Should_not_get_classroom_when_user_has_no_permission()
    {
        // Arrange
        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.GetClassroom(id: 1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    #endregion

    #region Validation errors

    [Test]
    public async Task Classrooms_GetClassroom_Should_not_get_classroom_not_found()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.GetClassroom(id: 99999);

        // Assert
        result.ShouldBeError(ClassroomNotFound.I);
    }

    [Test]
    public async Task Classrooms_GetClassroom_Should_not_get_other_institution_classroom()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        var otherClient = await _back.LoggedAsDirector();
        var otherCampus = await otherClient.CreateCampus().Success();
        var otherClassroom = await otherClient.CreateClassroom(otherCampus.Id).Success();

        // Act
        var result = await client.GetClassroom(otherClassroom.Id);

        // Assert
        result.ShouldBeError(ClassroomNotFound.I);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task Classrooms_GetClassroom_Should_get_classroom()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var campus = await client.CreateCampus(name: "Campus Agreste").Success();
        var classroom = await client.CreateClassroom(campus.Id, name: "Sala 05", capacity: 40).Success();

        // Act
        var result = await client.GetClassroom(classroom.Id);

        // Assert
        var found = result.Success;
        found.Id.Should().Be(classroom.Id);
        found.Name.Should().Be("Sala 05");
        found.CampusId.Should().Be(campus.Id);
        found.Campus.Should().Be("Campus Agreste");
        found.Capacity.Should().Be(40);
        found.Schedules.Should().BeEmpty();
        found.ClassesCount.Should().Be(0);
        found.WeeklyHours.Should().Be(0);
        found.PeakStudents.Should().Be(0);
    }

    [Test]
    public async Task Classrooms_GetClassroom_Should_get_classroom_with_allocated_classes()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var campus = await client.CreateCampus().Success();
        var classroom = await client.CreateClassroom(campus.Id, capacity: 40).Success();
        var discipline = await client.CreateDiscipline().Success();
        var period = await client.CreateAcademicPeriod().Success();
        var @class = await client.CreateClass(discipline.Id, period.Id).Success();

        await client.UpdateClassSchedules(@class.Id,
        [
            (Day.Monday, Hour.H07_00, Hour.H10_00, null),
            (Day.Wednesday, Hour.H07_00, Hour.H09_00, null),
        ]);

        // Não há endpoint que aloque uma sala a um horário ainda, então o vínculo
        // é feito direto no banco.
        await using (var ctx = _back.GetDbContext())
        {
            var schedules = await ctx.Schedules.Where(s => s.ClassId == @class.Id).ToListAsync();
            foreach (var schedule in schedules) schedule.ClassroomId = classroom.Id;
            await ctx.SaveChangesAsync();
        }

        // Act
        var result = await client.GetClassroom(classroom.Id);

        // Assert
        var found = result.Success;
        found.Schedules.Should().HaveCount(2);
        found.ClassesCount.Should().Be(1);
        found.WeeklyHours.Should().Be(5M);
        found.PeakStudents.Should().Be(0);

        found.Schedules[0].ClassId.Should().Be(@class.Id);
        found.Schedules[0].Discipline.Should().Be("Geometria");
        found.Schedules[0].Period.Should().Be("2024.1");
        found.Schedules[0].Status.Should().Be(ClassStatus.OnPreEnrollment);
        found.Schedules[0].Students.Should().Be(0);
        found.Schedules[0].Day.Should().Be(Day.Monday);
    }

    [Test]
    public async Task Classrooms_GetClassroom_Should_get_classroom_with_enrolled_students_count()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var campus = await client.CreateCampus().Success();
        var classroom = await client.CreateClassroom(campus.Id, capacity: 40).Success();
        var discipline = await client.CreateDiscipline().Success();
        var period = await client.CreateAcademicPeriod().Success();
        var @class = await client.CreateClass(discipline.Id, period.Id).Success();
        var student = await client.CreateStudent(DataGen.UserName, DataGen.Email).Success();

        await client.UpdateClassSchedules(@class.Id, [(Day.Monday, Hour.H07_00, Hour.H10_00, null)]);

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        await client.CreateEnrollmentPeriod(startAt: today.AddDays(-2), endAt: today.AddDays(2));
        await client.ReleaseClassForEnrollment(@class.Id);
        await client.AssignStudentToClass(student.Id, @class.Id);

        await using (var ctx = _back.GetDbContext())
        {
            var schedules = await ctx.Schedules.Where(s => s.ClassId == @class.Id).ToListAsync();
            foreach (var schedule in schedules) schedule.ClassroomId = classroom.Id;
            await ctx.SaveChangesAsync();
        }

        // Act
        var result = await client.GetClassroom(classroom.Id);

        // Assert
        var found = result.Success;
        found.PeakStudents.Should().Be(1);
        found.Schedules.Should().ContainSingle();
        found.Schedules[0].Students.Should().Be(1);
        found.Schedules[0].Status.Should().Be(ClassStatus.OnEnrollment);
    }

    #endregion
}
