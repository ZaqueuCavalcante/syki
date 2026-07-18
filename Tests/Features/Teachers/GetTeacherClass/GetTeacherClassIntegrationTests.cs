namespace Estud.Tests.Integration;

public partial class IntegrationTests
{
    #region Authentication

    [Test]
    public async Task Teachers_GetTeacherClass_Should_not_get_class_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.GetTeacherClass(1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Authorization

    [Test]
    public async Task Teachers_GetTeacherClass_Should_not_get_class_when_user_is_not_a_teacher()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.GetTeacherClass(1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    #endregion

    #region Validation errors

    [Test]
    public async Task Teachers_GetTeacherClass_Should_not_get_class_when_class_not_found()
    {
        // Arrange
        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.GetTeacherClass(999999);

        // Assert
        result.ShouldBeError(ClassNotFound.I);
    }

    [Test]
    public async Task Teachers_GetTeacherClass_Should_not_get_class_of_another_institution()
    {
        // Arrange
        var director = await _back.LoggedAsDirector();
        var discipline = (await director.CreateDiscipline()).Success;
        var period = (await director.CreateAcademicPeriod()).Success;
        var @class = (await director.CreateClass(discipline.Id, period.Id)).Success;

        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.GetTeacherClass(@class.Id);

        // Assert
        result.ShouldBeError(ClassNotFound.I);
    }

    [Test]
    public async Task Teachers_GetTeacherClass_Should_not_get_class_when_teacher_is_not_assigned_to_it()
    {
        // Arrange
        var director = await _back.LoggedAsDirector();

        var email = DataGen.Email;
        await director.CreateTeacher(DataGen.UserName, email);

        var discipline = (await director.CreateDiscipline()).Success;
        var period = (await director.CreateAcademicPeriod()).Success;
        var @class = (await director.CreateClass(discipline.Id, period.Id)).Success;

        var client = await _back.LoginAs(email);

        // Act
        var result = await client.GetTeacherClass(@class.Id);

        // Assert
        result.ShouldBeError(TeacherNotAssignedToClass.I);
    }

    [Test]
    public async Task Teachers_GetTeacherClass_Should_not_get_class_of_another_teacher()
    {
        // Arrange
        var director = await _back.LoggedAsDirector();

        var email = DataGen.Email;
        await director.CreateTeacher(DataGen.UserName, email);
        var otherTeacher = (await director.CreateTeacher(DataGen.UserName, DataGen.Email)).Success;

        var discipline = (await director.CreateDiscipline()).Success;
        await director.AssignDisciplinesToTeacher(otherTeacher.Id, [discipline.Id]);

        var period = (await director.CreateAcademicPeriod()).Success;
        var @class = (await director.CreateClass(discipline.Id, period.Id)).Success;

        var client = await _back.LoginAs(email);

        // Act
        var result = await client.GetTeacherClass(@class.Id);

        // Assert
        result.ShouldBeError(TeacherNotAssignedToClass.I);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task Teachers_GetTeacherClass_Should_get_class_details()
    {
        // Arrange
        var director = await _back.LoggedAsDirector();

        var email = DataGen.Email;
        var teacher = (await director.CreateTeacher(DataGen.UserName, email)).Success;

        var discipline = (await director.CreateDiscipline()).Success;
        await director.AssignDisciplinesToTeacher(teacher.Id, [discipline.Id]);

        var period = (await director.CreateAcademicPeriod()).Success;
        var @class = (await director.CreateClass(discipline.Id, period.Id)).Success;
        await director.AssignTeachersToClass(@class.Id, [teacher.Id]);

        var client = await _back.LoginAs(email);

        // Act
        var result = await client.GetTeacherClass(@class.Id);

        // Assert
        var details = result.Success;
        details.Id.Should().Be(@class.Id);
        details.Discipline.Should().Be("Geometria");
        details.Period.Should().Be("2024.1");
        details.Vacancies.Should().Be(40);
        details.Status.Should().Be(ClassStatus.OnPreEnrollment);
        details.Schedules.Should().BeEmpty();
        details.Students.Should().BeEmpty();
    }

    [Test]
    public async Task Teachers_GetTeacherClass_Should_get_class_with_enrolled_students()
    {
        // Arrange
        var director = await _back.LoggedAsDirector();

        var email = DataGen.Email;
        var teacher = (await director.CreateTeacher(DataGen.UserName, email)).Success;

        var discipline = (await director.CreateDiscipline()).Success;
        await director.AssignDisciplinesToTeacher(teacher.Id, [discipline.Id]);

        var period = (await director.CreateAcademicPeriod()).Success;
        var @class = (await director.CreateClass(discipline.Id, period.Id)).Success;
        await director.AssignTeachersToClass(@class.Id, [teacher.Id]);

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        await director.CreateEnrollmentPeriod(startAt: today.AddDays(-2), endAt: today.AddDays(2));
        await director.ReleaseClassForEnrollment(@class.Id);

        var studentName = DataGen.UserName;
        var student = (await director.CreateStudent(studentName, DataGen.Email)).Success;
        await director.AssignStudentToClass(student.Id, @class.Id);

        await director.StartClass(@class.Id);

        var client = await _back.LoginAs(email);

        // Act
        var result = await client.GetTeacherClass(@class.Id);

        // Assert
        var details = result.Success;
        details.Status.Should().Be(ClassStatus.Started);
        details.Students.Should().ContainSingle();
        details.Students[0].Id.Should().Be(student.Id);
        details.Students[0].Name.Should().Be(studentName);
        details.Students[0].Status.Should().Be(StudentClassStatus.Matriculado);
    }

    #endregion
}
