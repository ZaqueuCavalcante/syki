namespace Estud.Tests.Integration;

public partial class IntegrationTests
{
    #region Authentication

    [Test]
    public async Task Students_AssignStudentToClass_Should_not_assign_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.AssignStudentToClass(1, 1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Authorization

    [Test]
    public async Task Students_AssignStudentToClass_Should_not_assign_when_user_has_no_permission()
    {
        // Arrange
        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.AssignStudentToClass(1, 1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    #endregion

    #region Validation errors

    [Test]
    public async Task Students_AssignStudentToClass_Should_not_assign_when_student_not_found()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.AssignStudentToClass(999999, 1);

        // Assert
        result.ShouldBeError(StudentNotFound.I);
    }

    [Test]
    public async Task Students_AssignStudentToClass_Should_not_assign_when_class_not_found()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var student = await client.CreateStudent(DataGen.UserName, DataGen.Email).Success();

        // Act
        var result = await client.AssignStudentToClass(student.Id, 999999);

        // Assert
        result.ShouldBeError(ClassNotFound.I);
    }

    [Test]
    public async Task Students_AssignStudentToClass_Should_not_assign_when_class_is_not_on_enrollment()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var student = await client.CreateStudent(DataGen.UserName, DataGen.Email).Success();
        var discipline = await client.CreateDiscipline().Success();
        var period = await client.CreateAcademicPeriod().Success();
        var @class = await client.CreateClass(discipline.Id, period.Id).Success();

        // Act
        var result = await client.AssignStudentToClass(student.Id, @class.Id);

        // Assert
        result.ShouldBeError(ClassMustBeOnEnrollment.I);
    }

    [Test]
    public async Task Students_AssignStudentToClass_Should_not_assign_when_student_already_enrolled()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var student = await client.CreateStudent(DataGen.UserName, DataGen.Email).Success();
        var discipline = await client.CreateDiscipline().Success();
        var period = await client.CreateAcademicPeriod().Success();
        var @class = await client.CreateClass(discipline.Id, period.Id).Success();

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        await client.CreateEnrollmentPeriod(startAt: today.AddDays(-2), endAt: today.AddDays(2));
        await client.ReleaseClassForEnrollment(@class.Id);

        await client.AssignStudentToClass(student.Id, @class.Id);

        // Act
        var result = await client.AssignStudentToClass(student.Id, @class.Id);

        // Assert
        result.ShouldBeError(StudentAlreadyEnrolledInClass.I);
    }

    [Test]
    public async Task Students_AssignStudentToClass_Should_not_assign_when_class_has_no_vacancies()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var studentA = await client.CreateStudent(DataGen.UserName, DataGen.Email).Success();
        var studentB = await client.CreateStudent(DataGen.UserName, DataGen.Email).Success();
        var discipline = await client.CreateDiscipline().Success();
        var period = await client.CreateAcademicPeriod().Success();
        var @class = await client.CreateClass(discipline.Id, period.Id, vacancies: 1).Success();

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        await client.CreateEnrollmentPeriod(startAt: today.AddDays(-2), endAt: today.AddDays(2));
        await client.ReleaseClassForEnrollment(@class.Id);

        await client.AssignStudentToClass(studentA.Id, @class.Id);

        // Act
        var result = await client.AssignStudentToClass(studentB.Id, @class.Id);

        // Assert
        result.ShouldBeError(NoVacanciesInClass.I);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task Students_AssignStudentToClass_Should_assign_student_to_class()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var student = await client.CreateStudent(DataGen.UserName, DataGen.Email).Success();
        var discipline = await client.CreateDiscipline().Success();
        var period = await client.CreateAcademicPeriod().Success();
        var @class = await client.CreateClass(discipline.Id, period.Id).Success();

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        await client.CreateEnrollmentPeriod(startAt: today.AddDays(-2), endAt: today.AddDays(2));
        await client.ReleaseClassForEnrollment(@class.Id);

        // Act
        var result = await client.AssignStudentToClass(student.Id, @class.Id);

        // Assert
        result.ShouldBeSuccess();

        await using var db = _back.GetDbContext();
        var link = await db.ClassStudents.FirstAsync(x => x.ClassId == @class.Id && x.StudentId == student.Id);
        link.Status.Should().Be(StudentClassStatus.Matriculado);
    }

    #endregion
}
