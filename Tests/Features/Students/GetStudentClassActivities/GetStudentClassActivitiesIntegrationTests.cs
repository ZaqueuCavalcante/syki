using Estud.Tests.Integration.Clients;

namespace Estud.Tests.Integration;

public partial class IntegrationTests
{
    #region Authentication

    [Test]
    public async Task Students_GetStudentClassActivities_Should_not_get_activities_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.GetStudentClassActivities(1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Authorization

    [Test]
    public async Task Students_GetStudentClassActivities_Should_not_get_activities_when_user_is_not_a_student()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.GetStudentClassActivities(1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    [Test]
    public async Task Students_GetStudentClassActivities_Should_not_get_activities_when_user_is_a_teacher()
    {
        // Arrange
        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.GetStudentClassActivities(1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    #endregion

    #region Validation errors

    [Test]
    public async Task Students_GetStudentClassActivities_Should_not_get_activities_when_class_not_found()
    {
        // Arrange
        var director = await _back.LoggedAsDirector();

        var email = DataGen.Email;
        await director.CreateStudent(DataGen.UserName, email);

        var client = await _back.LoginAs(email);

        // Act
        var result = await client.GetStudentClassActivities(999999);

        // Assert
        result.ShouldBeError(ClassNotFound.I);
    }

    [Test]
    public async Task Students_GetStudentClassActivities_Should_not_get_activities_of_class_of_another_institution()
    {
        // Arrange
        var otherDirector = await _back.LoggedAsDirector();
        var (otherClassId, _) = await CreateClassWithTeacher(otherDirector);

        var director = await _back.LoggedAsDirector();

        var email = DataGen.Email;
        await director.CreateStudent(DataGen.UserName, email);

        var client = await _back.LoginAs(email);

        // Act
        var result = await client.GetStudentClassActivities(otherClassId);

        // Assert
        result.ShouldBeError(ClassNotFound.I);
    }

    [Test]
    public async Task Students_GetStudentClassActivities_Should_not_get_activities_when_student_is_not_enrolled_in_class()
    {
        // Arrange
        var director = await _back.LoggedAsDirector();
        var (classId, _) = await CreateClassWithTeacher(director);

        var email = DataGen.Email;
        await director.CreateStudent(DataGen.UserName, email);

        var client = await _back.LoginAs(email);

        // Act
        var result = await client.GetStudentClassActivities(classId);

        // Assert
        result.ShouldBeError(StudentNotEnrolledInClass.I);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task Students_GetStudentClassActivities_Should_get_empty_list_when_class_has_no_activities()
    {
        // Arrange
        var director = await _back.LoggedAsDirector();
        var (classId, _) = await CreateClassWithTeacher(director);

        var email = DataGen.Email;
        await EnrollStudentInClass(director, classId, email);

        var client = await _back.LoginAs(email);

        // Act
        var result = await client.GetStudentClassActivities(classId);

        // Assert
        result.Success.Activities.Should().BeEmpty();
    }

    [Test]
    public async Task Students_GetStudentClassActivities_Should_get_only_activities_of_the_class()
    {
        // Arrange
        var director = await _back.LoggedAsDirector();
        var (classId, teacherEmail) = await CreateClassWithTeacher(director);
        var (otherClassId, otherTeacherEmail) = await CreateClassWithTeacher(director);

        var email = DataGen.Email;
        await EnrollStudentInClass(director, classId, email);

        var teacherClient = await _back.LoginAs(teacherEmail);
        await teacherClient.CreateClassActivity(classId, ClassNoteType.N1, type: ClassActivityType.Work, weight: 25);
        await teacherClient.CreateClassActivity(classId, ClassNoteType.N2, type: ClassActivityType.Presentation, weight: 10);
        await teacherClient.CreateClassActivity(classId, ClassNoteType.N2, type: ClassActivityType.Work, weight: 30);

        var otherTeacherClient = await _back.LoginAs(otherTeacherEmail);
        await otherTeacherClient.CreateClassActivity(otherClassId, ClassNoteType.N1, type: ClassActivityType.Work, weight: 80);

        var client = await _back.LoginAs(email);

        // Act
        var result = await client.GetStudentClassActivities(classId);

        // Assert
        var activities = result.Success.Activities;
        activities.Should().HaveCount(3);
        activities.Should().OnlyContain(a => a.ClassId == classId);
        activities.Select(a => a.Note).Should().Equal(ClassNoteType.N1, ClassNoteType.N2, ClassNoteType.N2);
    }

    [Test]
    public async Task Students_GetStudentClassActivities_Should_get_activity_with_pending_work()
    {
        // Arrange
        var director = await _back.LoggedAsDirector();
        var (classId, teacherEmail) = await CreateClassWithTeacher(director);

        var email = DataGen.Email;
        await EnrollStudentInClass(director, classId, email);

        var teacherClient = await _back.LoginAs(teacherEmail);
        var dueDate = DateTime.UtcNow.AddDays(7).ToDateOnly();
        await teacherClient.CreateClassActivity(
            classId,
            ClassNoteType.N2,
            "Modelagem de Banco de Dados",
            "Modele um banco de dados para um sistema de gerenciamento de biblioteca.",
            ClassActivityType.Work,
            40,
            dueDate,
            Hour.H08_30
        );

        var client = await _back.LoginAs(email);

        // Act
        var result = await client.GetStudentClassActivities(classId);

        // Assert
        var activity = result.Success.Activities.Should().ContainSingle().Subject;
        activity.ClassId.Should().Be(classId);
        activity.Note.Should().Be(ClassNoteType.N2);
        activity.Title.Should().Be("Modelagem de Banco de Dados");
        activity.Description.Should().Be("Modele um banco de dados para um sistema de gerenciamento de biblioteca.");
        activity.Type.Should().Be(ClassActivityType.Work);
        activity.Status.Should().Be(ClassActivityStatus.Pending);
        activity.Weight.Should().Be(40);
        activity.DueDate.Should().Be(dueDate);
        activity.DueHour.Should().Be(Hour.H08_30);
        activity.WorkStatus.Should().Be(ClassActivityWorkStatus.Pending);
        activity.WorkLink.Should().BeNull();
        activity.Value.Should().Be(0);
        activity.PonderedValue.Should().Be(0);
    }

    [Test]
    public async Task Students_GetStudentClassActivities_Should_get_activity_with_delivered_work()
    {
        // Arrange
        var director = await _back.LoggedAsDirector();
        var (classId, teacherEmail) = await CreateClassWithTeacher(director);

        var email = DataGen.Email;
        await EnrollStudentInClass(director, classId, email);

        var teacherClient = await _back.LoginAs(teacherEmail);
        var activity = (await teacherClient.CreateClassActivity(classId, weight: 40)).Success;

        var client = await _back.LoginAs(email);
        await client.CreateClassActivityWork(activity.Id, "https://github.com/ZaqueuCavalcante/estud");

        // Act
        var result = await client.GetStudentClassActivities(classId);

        // Assert
        var item = result.Success.Activities.Should().ContainSingle().Subject;
        item.Id.Should().Be(activity.Id);
        item.WorkStatus.Should().Be(ClassActivityWorkStatus.Delivered);
        item.WorkLink.Should().Be("https://github.com/ZaqueuCavalcante/estud");
    }

    #endregion

    private async Task<(int ClassId, string TeacherEmail)> CreateClassWithTeacher(TestsHttpClient director)
    {
        var teacherEmail = DataGen.Email;
        var teacher = (await director.CreateTeacher(DataGen.UserName, teacherEmail)).Success;

        var discipline = (await director.CreateDiscipline()).Success;
        await director.AssignDisciplinesToTeacher(teacher.Id, [discipline.Id]);

        var period = (await director.CreateAcademicPeriod()).Success;
        var @class = (await director.CreateClass(discipline.Id, period.Id, teacherId: teacher.Id)).Success;

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        await director.CreateEnrollmentPeriod(startAt: today.AddDays(-2), endAt: today.AddDays(2));
        await director.ReleaseClassForEnrollment(@class.Id);

        return (@class.Id, teacherEmail);
    }

    private static async Task EnrollStudentInClass(TestsHttpClient director, int classId, string studentEmail)
    {
        var student = (await director.CreateStudent(DataGen.UserName, studentEmail)).Success;
        await director.AssignStudentToClass(student.Id, classId);
    }
}
