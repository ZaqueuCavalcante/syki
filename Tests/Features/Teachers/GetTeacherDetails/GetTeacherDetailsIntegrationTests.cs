namespace Estud.Tests.Integration;

public partial class IntegrationTests
{
    #region Authentication

    [Test]
    public async Task Teachers_GetTeacherDetails_Should_not_get_teacher_details_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.GetTeacherDetails(1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Authorization

    [Test]
    public async Task Teachers_GetTeacherDetails_Should_not_get_teacher_details_when_user_has_no_permission()
    {
        // Arrange
        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.GetTeacherDetails(1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    #endregion

    #region Validation errors

    [Test]
    public async Task Teachers_GetTeacherDetails_Should_not_get_teacher_details_when_teacher_does_not_exist()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.GetTeacherDetails(999999);

        // Assert
        result.ShouldBeError(TeacherNotFound.I);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task Teachers_GetTeacherDetails_Should_get_teacher_details_without_classes()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var email = DataGen.Email;
        var teacher = await client.CreateTeacher("Ana Lima", email).Success();

        // Act
        var result = await client.GetTeacherDetails(teacher.Id);

        // Assert
        var details = result.Success;
        details.Id.Should().Be(teacher.Id);
        details.Name.Should().Be("Ana Lima");
        details.Email.Should().Be(email);
        details.Campi.Should().BeEmpty();
        details.Disciplines.Should().BeEmpty();
        details.Classes.Should().BeEmpty();
    }

    [Test]
    public async Task Teachers_GetTeacherDetails_Should_get_teacher_campi_and_disciplines()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var teacher = await client.CreateTeacher("Ana Lima", DataGen.Email).Success();
        var campus = await client.CreateCampus().Success();
        var discipline = await client.CreateDiscipline().Success();

        await client.AssignCampiToTeacher(teacher.Id, [campus.Id]);
        await client.AssignDisciplinesToTeacher(teacher.Id, [discipline.Id]);

        // Act
        var result = await client.GetTeacherDetails(teacher.Id);

        // Assert
        var details = result.Success;
        details.Campi.Select(c => c.Id).Should().Equal(campus.Id);
        details.Disciplines.Select(d => d.Id).Should().Equal(discipline.Id);
    }

    [Test]
    public async Task Teachers_GetTeacherDetails_Should_get_the_teacher_classes_with_his_schedules()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var discipline = await client.CreateDiscipline().Success();
        var period = await client.CreateAcademicPeriod().Success();
        var @class = await client.CreateClass(discipline.Id, period.Id).Success();

        var teacher = await client.CreateTeacher("Ana Lima", DataGen.Email).Success();
        await client.AssignDisciplinesToTeacher(teacher.Id, [discipline.Id]);
        await client.AssignTeachersToClass(@class.Id, [teacher.Id]);

        await client.UpdateClassSchedules(@class.Id,
        [
            (Day.Monday, Hour.H07_00, Hour.H10_00, teacher.Id),
        ]);

        // Act
        var result = await client.GetTeacherDetails(teacher.Id);

        // Assert
        var details = result.Success;
        details.Classes.Should().ContainSingle();

        var found = details.Classes[0];
        found.Id.Should().Be(@class.Id);
        found.Discipline.Should().Be("Geometria");
        found.Period.Should().Be("2024.1");
        found.Status.Should().Be(ClassStatus.OnPreEnrollment);
        found.Students.Should().Be(0);
        found.Schedules.Should().ContainSingle();
        found.Schedules[0].Day.Should().Be(Day.Monday);
        found.Schedules[0].Teacher.Should().Be("Ana Lima");
    }

    [Test]
    public async Task Teachers_GetTeacherDetails_Should_not_get_classes_of_other_teachers()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var discipline = await client.CreateDiscipline().Success();
        var period = await client.CreateAcademicPeriod().Success();
        var @class = await client.CreateClass(discipline.Id, period.Id).Success();

        var ana = await client.CreateTeacher("Ana Lima", DataGen.Email).Success();
        var chico = await client.CreateTeacher("Chico Ferreira", DataGen.Email).Success();
        await client.AssignDisciplinesToTeacher(ana.Id, [discipline.Id]);
        await client.AssignTeachersToClass(@class.Id, [ana.Id]);

        // Act
        var result = await client.GetTeacherDetails(chico.Id);

        // Assert
        result.Success.Classes.Should().BeEmpty();
    }

    #endregion
}
