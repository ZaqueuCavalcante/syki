namespace Estud.Tests.Integration;

public partial class IntegrationTests
{
    #region Authentication

    [Test]
    public async Task Students_GetStudentClass_Should_not_get_class_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.GetStudentClass(1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Authorization

    [Test]
    public async Task Students_GetStudentClass_Should_not_get_class_when_user_is_not_a_student()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.GetStudentClass(1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    [Test]
    public async Task Students_GetStudentClass_Should_not_get_class_when_user_is_a_teacher()
    {
        // Arrange
        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.GetStudentClass(1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    #endregion

    #region Validation errors

    [Test]
    public async Task Students_GetStudentClass_Should_not_get_class_when_class_not_found()
    {
        // Arrange
        var director = await _back.LoggedAsDirector();

        var email = DataGen.Email;
        await director.CreateStudent(DataGen.UserName, email);

        var client = await _back.LoginAs(email);

        // Act
        var result = await client.GetStudentClass(999999);

        // Assert
        result.ShouldBeError(ClassNotFound.I);
    }

    [Test]
    public async Task Students_GetStudentClass_Should_not_get_class_of_another_institution()
    {
        // Arrange
        var otherDirector = await _back.LoggedAsDirector();
        var discipline = await otherDirector.CreateDiscipline().Success();
        var period = await otherDirector.CreateAcademicPeriod().Success();
        var @class = await otherDirector.CreateClass(discipline.Id, period.Id).Success();

        var director = await _back.LoggedAsDirector();

        var email = DataGen.Email;
        await director.CreateStudent(DataGen.UserName, email);

        var client = await _back.LoginAs(email);

        // Act
        var result = await client.GetStudentClass(@class.Id);

        // Assert
        result.ShouldBeError(ClassNotFound.I);
    }

    [Test]
    public async Task Students_GetStudentClass_Should_not_get_class_when_student_is_not_enrolled_in_it()
    {
        // Arrange
        var director = await _back.LoggedAsDirector();

        var discipline = await director.CreateDiscipline().Success();
        var period = await director.CreateAcademicPeriod().Success();
        var @class = await director.CreateClass(discipline.Id, period.Id).Success();

        var email = DataGen.Email;
        await director.CreateStudent(DataGen.UserName, email);

        var client = await _back.LoginAs(email);

        // Act
        var result = await client.GetStudentClass(@class.Id);

        // Assert
        result.ShouldBeError(StudentNotEnrolledInClass.I);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task Students_GetStudentClass_Should_get_class_details()
    {
        // Arrange
        var director = await _back.LoggedAsDirector();

        var teacherName = DataGen.UserName;
        var teacher = await director.CreateTeacher(teacherName, DataGen.Email).Success();

        var discipline = await director.CreateDiscipline().Success();
        await director.AssignDisciplinesToTeacher(teacher.Id, [discipline.Id]);

        var period = await director.CreateAcademicPeriod().Success();
        var @class = await director.CreateClass(discipline.Id, period.Id).Success();
        await director.UpdateClassTeachers(@class.Id, [teacher.Id]);
        await director.UpdateClassSchedules(@class.Id, [(Day.Monday, Hour.H07_00, Hour.H10_00, null)]);

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var enrollment = await director.CreateEnrollmentPeriod(startAt: today.AddDays(-2), endAt: today.AddDays(2)).Success();
        await director.ReleaseClassForEnrollment(@class.Id);
        await director.UpdateEnrollmentPeriod(enrollment.Id, startAt: today.AddDays(-10), endAt: today.AddDays(-5));

        var email = DataGen.Email;
        var student = await director.CreateStudent(DataGen.UserName, email).Success();
        await director.AssignStudentToClass(student.Id, @class.Id);

        await director.StartClass(@class.Id);

        var client = await _back.LoginAs(email);

        // Act
        var result = await client.GetStudentClass(@class.Id);

        // Assert
        var details = result.Success;
        details.Id.Should().Be(@class.Id);
        details.Discipline.Should().Be("Geometria");
        details.Period.Should().Be("2024.1");
        details.Status.Should().Be(ClassStatus.Started);
        details.MyStatus.Should().Be(StudentClassStatus.Matriculado);
        details.Schedules.Should().ContainSingle();
    }

    #endregion
}
