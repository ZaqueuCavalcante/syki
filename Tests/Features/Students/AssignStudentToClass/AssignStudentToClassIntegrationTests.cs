using Estud.Tests.Integration.Clients;

namespace Estud.Tests.Integration;

public partial class IntegrationTests
{
    private async Task<int> CreateOnEnrollmentClass(TestsHttpClient client, int vacancies = 40)
    {
        var discipline = (await client.CreateDiscipline()).Success;
        var period = (await client.CreateAcademicPeriod()).Success;
        var @class = (await client.CreateClass(discipline.Id, period.Id, vacancies: vacancies)).Success;

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        await client.CreateEnrollmentPeriod(startAt: today.AddDays(-2), endAt: today.AddDays(2));
        await client.ReleaseClassForEnrollment(@class.Id);

        return @class.Id;
    }

    private async Task FinalizeEnrollmentPeriodOf(int classId)
    {
        await using var ctx = _back.GetDbContext();

        var institutionId = await ctx.Classes
            .Where(c => c.Id == classId)
            .Select(c => c.InstitutionId)
            .FirstAsync();

        var enrollmentPeriod = await ctx.EnrollmentPeriods.FirstAsync(p => p.InstitutionId == institutionId);
        enrollmentPeriod.EndAt = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(-1);
        await ctx.SaveChangesAsync();
    }

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
        var student = (await client.CreateStudent(DataGen.UserName, DataGen.Email)).Success;

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
        var student = (await client.CreateStudent(DataGen.UserName, DataGen.Email)).Success;
        var discipline = (await client.CreateDiscipline()).Success;
        var period = (await client.CreateAcademicPeriod()).Success;
        var @class = (await client.CreateClass(discipline.Id, period.Id)).Success;

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
        var student = (await client.CreateStudent(DataGen.UserName, DataGen.Email)).Success;
        var classId = await CreateOnEnrollmentClass(client);
        await client.AssignStudentToClass(student.Id, classId);

        // Act
        var result = await client.AssignStudentToClass(student.Id, classId);

        // Assert
        result.ShouldBeError(StudentAlreadyEnrolledInClass.I);
    }

    [Test]
    public async Task Students_AssignStudentToClass_Should_not_assign_when_class_has_no_vacancies()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();
        var studentA = (await client.CreateStudent(DataGen.UserName, DataGen.Email)).Success;
        var studentB = (await client.CreateStudent(DataGen.UserName, DataGen.Email)).Success;
        var classId = await CreateOnEnrollmentClass(client, vacancies: 1);
        await client.AssignStudentToClass(studentA.Id, classId);

        // Act
        var result = await client.AssignStudentToClass(studentB.Id, classId);

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
        var student = (await client.CreateStudent(DataGen.UserName, DataGen.Email)).Success;
        var classId = await CreateOnEnrollmentClass(client);

        // Act
        var result = await client.AssignStudentToClass(student.Id, classId);

        // Assert
        result.ShouldBeSuccess();

        await using var db = _back.GetDbContext();
        var link = await db.ClassStudents.FirstAsync(x => x.ClassId == classId && x.StudentId == student.Id);
        link.Status.Should().Be(StudentClassStatus.Matriculado);
    }

    #endregion
}
