namespace Estud.Tests.Integration;

public partial class IntegrationTests
{
    #region Authentication

    [Test]
    public async Task Teachers_GetTeacherCurrentClasses_Should_not_get_current_classes_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.GetTeacherCurrentClasses();

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Authorization

    [Test]
    public async Task Teachers_GetTeacherCurrentClasses_Should_not_get_current_classes_when_user_is_not_a_teacher()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.GetTeacherCurrentClasses();

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task Teachers_GetTeacherCurrentClasses_Should_get_current_classes_ordered_by_discipline_name()
    {
        // Arrange
        var director = await _back.LoggedAsDirector();

        var email = DataGen.Email;
        var teacher = await director.CreateTeacher(DataGen.UserName, email).Success();

        var geometria = await director.CreateDiscipline("Geometria").Success();
        var algebra = await director.CreateDiscipline("Álgebra").Success();
        await director.AssignDisciplinesToTeacher(teacher.Id, [geometria.Id, algebra.Id]);

        var period = await director.CreateAcademicPeriod().Success();
        var geometriaClass = await director.CreateClass(geometria.Id, period.Id).Success();
        var algebraClass = await director.CreateClass(algebra.Id, period.Id).Success();

        await director.AssignTeachersToClass(geometriaClass.Id, [teacher.Id]);
        await director.AssignTeachersToClass(algebraClass.Id, [teacher.Id]);
        await director.UpdateClassSchedules(geometriaClass.Id, [(Day.Monday, Hour.H07_00, Hour.H10_00, null)]);
        await director.UpdateClassSchedules(algebraClass.Id, [(Day.Tuesday, Hour.H07_00, Hour.H10_00, null)]);

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var enrollment = await director.CreateEnrollmentPeriod(startAt: today.AddDays(-2), endAt: today.AddDays(2)).Success();
        await director.ReleaseClassForEnrollment(geometriaClass.Id);
        await director.ReleaseClassForEnrollment(algebraClass.Id);
        await director.UpdateEnrollmentPeriod(enrollment.Id, startAt: today.AddDays(-10), endAt: today.AddDays(-5));

        await director.StartClass(geometriaClass.Id);
        await director.StartClass(algebraClass.Id);

        var client = await _back.LoginAs(email);

        // Act
        var result = await client.GetTeacherCurrentClasses();

        // Assert
        var classes = result.Success.Classes;
        classes.Should().HaveCount(2);
        classes[0].Id.Should().Be(algebraClass.Id);
        classes[0].Name.Should().Be("Álgebra");
        classes[1].Id.Should().Be(geometriaClass.Id);
        classes[1].Name.Should().Be("Geometria");
    }

    [Test]
    public async Task Teachers_GetTeacherCurrentClasses_Should_not_get_classes_that_are_not_started()
    {
        // Arrange
        var director = await _back.LoggedAsDirector();

        var email = DataGen.Email;
        var teacher = await director.CreateTeacher(DataGen.UserName, email).Success();

        var discipline = await director.CreateDiscipline().Success();
        await director.AssignDisciplinesToTeacher(teacher.Id, [discipline.Id]);

        var period = await director.CreateAcademicPeriod().Success();
        await director.CreateClass(discipline.Id, period.Id);

        var client = await _back.LoginAs(email);

        // Act
        var result = await client.GetTeacherCurrentClasses();

        // Assert
        result.Success.Classes.Should().BeEmpty();
    }

    [Test]
    public async Task Teachers_GetTeacherCurrentClasses_Should_not_get_classes_of_another_teacher()
    {
        // Arrange
        var director = await _back.LoggedAsDirector();

        var email = DataGen.Email;
        var teacher = await director.CreateTeacher(DataGen.UserName, email).Success();
        var otherTeacher = await director.CreateTeacher(DataGen.UserName, DataGen.Email).Success();

        var discipline = await director.CreateDiscipline().Success();
        await director.AssignDisciplinesToTeacher(otherTeacher.Id, [discipline.Id]);

        var period = await director.CreateAcademicPeriod().Success();
        var otherClass = await director.CreateClass(discipline.Id, period.Id).Success();
        await director.AssignTeachersToClass(otherClass.Id, [otherTeacher.Id]);
        await director.UpdateClassSchedules(otherClass.Id, [(Day.Monday, Hour.H07_00, Hour.H10_00, null)]);

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var enrollment = await director.CreateEnrollmentPeriod(startAt: today.AddDays(-2), endAt: today.AddDays(2)).Success();
        await director.ReleaseClassForEnrollment(otherClass.Id);
        await director.UpdateEnrollmentPeriod(enrollment.Id, startAt: today.AddDays(-10), endAt: today.AddDays(-5));

        await director.StartClass(otherClass.Id);

        var client = await _back.LoginAs(email);

        // Act
        var result = await client.GetTeacherCurrentClasses();

        // Assert
        result.Success.Classes.Should().BeEmpty();
    }

    #endregion
}
