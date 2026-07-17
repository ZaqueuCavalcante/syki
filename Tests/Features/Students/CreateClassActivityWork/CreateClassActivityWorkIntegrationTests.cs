using Estud.Tests.Integration.Clients;

namespace Estud.Tests.Integration;

public partial class IntegrationTests
{
    #region Authentication

    [Test]
    public async Task Students_CreateClassActivityWork_Should_not_create_work_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.CreateClassActivityWork(1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Authorization

    [Test]
    public async Task Students_CreateClassActivityWork_Should_not_create_work_when_user_is_not_a_student()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.CreateClassActivityWork(1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    [Test]
    public async Task Students_CreateClassActivityWork_Should_not_create_work_when_user_is_a_teacher()
    {
        // Arrange
        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.CreateClassActivityWork(1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    #endregion

    #region Validation errors

    [TestCase("")]
    [TestCase(null)]
    public async Task Students_CreateClassActivityWork_Should_not_create_work_with_invalid_link(string? link)
    {
        // Arrange
        var director = await _back.LoggedAsDirector();

        var email = DataGen.Email;
        await director.CreateStudent(DataGen.UserName, email);

        var client = await _back.LoginAs(email);

        // Act
        var result = await client.CreateClassActivityWork(1, link);

        // Assert
        result.ShouldBeError(InvalidClassActivityWorkLink.I);
    }

    [Test]
    public async Task Students_CreateClassActivityWork_Should_not_create_work_when_activity_not_found()
    {
        // Arrange
        var director = await _back.LoggedAsDirector();

        var email = DataGen.Email;
        await director.CreateStudent(DataGen.UserName, email);

        var client = await _back.LoginAs(email);

        // Act
        var result = await client.CreateClassActivityWork(999999);

        // Assert
        result.ShouldBeError(ClassActivityNotFound.I);
    }

    [Test]
    public async Task Students_CreateClassActivityWork_Should_not_create_work_on_activity_of_another_institution()
    {
        // Arrange
        var otherDirector = await _back.LoggedAsDirector();
        var otherStudentEmail = DataGen.Email;
        var activityId = await CreateClassActivityWithEnrolledStudent(otherDirector, otherStudentEmail);

        var director = await _back.LoggedAsDirector();

        var email = DataGen.Email;
        await director.CreateStudent(DataGen.UserName, email);

        var client = await _back.LoginAs(email);

        // Act
        var result = await client.CreateClassActivityWork(activityId);

        // Assert
        result.ShouldBeError(ClassActivityNotFound.I);
    }

    [Test]
    public async Task Students_CreateClassActivityWork_Should_not_create_work_when_student_is_not_enrolled_in_class()
    {
        // Arrange
        var director = await _back.LoggedAsDirector();
        var activityId = await CreateClassActivityWithEnrolledStudent(director, DataGen.Email);

        var email = DataGen.Email;
        await director.CreateStudent(DataGen.UserName, email);

        var client = await _back.LoginAs(email);

        // Act
        var result = await client.CreateClassActivityWork(activityId);

        // Assert
        result.ShouldBeError(StudentNotEnrolledInClass.I);
    }

    [Test]
    public async Task Students_CreateClassActivityWork_Should_not_create_work_when_student_enrolled_after_activity_creation()
    {
        // Arrange
        var director = await _back.LoggedAsDirector();

        var teacherEmail = DataGen.Email;
        var teacher = (await director.CreateTeacher(DataGen.UserName, teacherEmail)).Success;

        var discipline = (await director.CreateDiscipline()).Success;
        await director.AssignDisciplinesToTeacher(teacher.Id, [discipline.Id]);

        var period = (await director.CreateAcademicPeriod()).Success;
        var @class = (await director.CreateClass(discipline.Id, period.Id)).Success;

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        await director.CreateEnrollmentPeriod(startAt: today.AddDays(-2), endAt: today.AddDays(2));
        await director.ReleaseClassForEnrollment(@class.Id);

        var teacherClient = await _back.LoginAs(teacherEmail);
        var activity = (await teacherClient.CreateClassActivity(@class.Id)).Success;

        var email = DataGen.Email;
        var student = (await director.CreateStudent(DataGen.UserName, email)).Success;
        await director.AssignStudentToClass(student.Id, @class.Id);

        var client = await _back.LoginAs(email);

        // Act
        var result = await client.CreateClassActivityWork(activity.Id);

        // Assert
        result.ShouldBeError(ClassActivityWorkNotFound.I);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task Students_CreateClassActivityWork_Should_create_work()
    {
        // Arrange
        var director = await _back.LoggedAsDirector();

        var email = DataGen.Email;
        var activityId = await CreateClassActivityWithEnrolledStudent(director, email);

        var client = await _back.LoginAs(email);

        // Act
        var result = await client.CreateClassActivityWork(activityId, "https://github.com/ZaqueuCavalcante/estud");

        // Assert
        var work = result.Success;
        work.Id.Should().BeGreaterThan(0);

        await using var ctx = _back.GetDbContext();
        var saved = await ctx.ClassActivityWorks.FirstAsync(x => x.Id == work.Id);
        saved.ClassActivityId.Should().Be(activityId);
        saved.Status.Should().Be(ClassActivityWorkStatus.Delivered);
        saved.Link.Should().Be("https://github.com/ZaqueuCavalcante/estud");
    }

    [Test]
    public async Task Students_CreateClassActivityWork_Should_update_link_when_work_is_delivered_again()
    {
        // Arrange
        var director = await _back.LoggedAsDirector();

        var email = DataGen.Email;
        var activityId = await CreateClassActivityWithEnrolledStudent(director, email);

        var client = await _back.LoginAs(email);
        await client.CreateClassActivityWork(activityId, "https://github.com/ZaqueuCavalcante/estud");

        // Act
        var result = await client.CreateClassActivityWork(activityId, "https://github.com/ZaqueuCavalcante/estud/pulls");

        // Assert
        var work = result.Success;

        await using var ctx = _back.GetDbContext();
        var works = await ctx.ClassActivityWorks.Where(x => x.ClassActivityId == activityId).ToListAsync();
        works.Should().ContainSingle();
    }

    #endregion

    private async Task<int> CreateClassActivityWithEnrolledStudent(TestsHttpClient director, string studentEmail)
    {
        var teacherEmail = DataGen.Email;
        var teacher = (await director.CreateTeacher(DataGen.UserName, teacherEmail)).Success;

        var discipline = (await director.CreateDiscipline()).Success;
        await director.AssignDisciplinesToTeacher(teacher.Id, [discipline.Id]);

        var period = (await director.CreateAcademicPeriod()).Success;
        var @class = (await director.CreateClass(discipline.Id, period.Id)).Success;

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        await director.CreateEnrollmentPeriod(startAt: today.AddDays(-2), endAt: today.AddDays(2));
        await director.ReleaseClassForEnrollment(@class.Id);

        var student = (await director.CreateStudent(DataGen.UserName, studentEmail)).Success;
        await director.AssignStudentToClass(student.Id, @class.Id);

        var teacherClient = await _back.LoginAs(teacherEmail);
        var activity = (await teacherClient.CreateClassActivity(@class.Id)).Success;

        return activity.Id;
    }
}
