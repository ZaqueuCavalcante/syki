namespace Estud.Tests.Integration;

public partial class IntegrationTests
{
    #region Authentication

    [Test]
    public async Task Students_GetStudentClassActivity_Should_not_get_activity_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.GetStudentClassActivity(1, 1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Authorization

    [Test]
    public async Task Students_GetStudentClassActivity_Should_not_get_activity_when_user_is_not_a_student()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.GetStudentClassActivity(1, 1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    [Test]
    public async Task Students_GetStudentClassActivity_Should_not_get_activity_when_user_is_a_teacher()
    {
        // Arrange
        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.GetStudentClassActivity(1, 1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    #endregion

    #region Validation errors

    [Test]
    public async Task Students_GetStudentClassActivity_Should_not_get_activity_when_class_not_found()
    {
        // Arrange
        var director = await _back.LoggedAsDirector();

        var email = DataGen.Email;
        await director.CreateStudent(DataGen.UserName, email);

        var client = await _back.LoginAs(email);

        // Act
        var result = await client.GetStudentClassActivity(999999, 999999);

        // Assert
        result.ShouldBeError(ClassNotFound.I);
    }

    [Test]
    public async Task Students_GetStudentClassActivity_Should_not_get_activity_of_class_of_another_institution()
    {
        // Arrange
        var otherDirector = await _back.LoggedAsDirector();
        var (otherClassId, otherTeacherEmail) = await CreateClassWithTeacher(otherDirector);

        var otherTeacherClient = await _back.LoginAs(otherTeacherEmail);
        var otherActivity = await otherTeacherClient.CreateClassActivity(otherClassId).Success();

        var director = await _back.LoggedAsDirector();

        var email = DataGen.Email;
        await director.CreateStudent(DataGen.UserName, email);

        var client = await _back.LoginAs(email);

        // Act
        var result = await client.GetStudentClassActivity(otherClassId, otherActivity.Id);

        // Assert
        result.ShouldBeError(ClassNotFound.I);
    }

    [Test]
    public async Task Students_GetStudentClassActivity_Should_not_get_activity_when_student_is_not_enrolled_in_class()
    {
        // Arrange
        var director = await _back.LoggedAsDirector();
        var (classId, teacherEmail) = await CreateClassWithTeacher(director);

        var teacherClient = await _back.LoginAs(teacherEmail);
        var activity = await teacherClient.CreateClassActivity(classId).Success();

        var email = DataGen.Email;
        await director.CreateStudent(DataGen.UserName, email);

        var client = await _back.LoginAs(email);

        // Act
        var result = await client.GetStudentClassActivity(classId, activity.Id);

        // Assert
        result.ShouldBeError(StudentNotEnrolledInClass.I);
    }

    [Test]
    public async Task Students_GetStudentClassActivity_Should_not_get_activity_when_activity_not_found()
    {
        // Arrange
        var director = await _back.LoggedAsDirector();
        var (classId, _) = await CreateClassWithTeacher(director);

        var email = DataGen.Email;
        await EnrollStudentInClass(director, classId, email);

        var client = await _back.LoginAs(email);

        // Act
        var result = await client.GetStudentClassActivity(classId, 999999);

        // Assert
        result.ShouldBeError(ClassActivityNotFound.I);
    }

    [Test]
    public async Task Students_GetStudentClassActivity_Should_not_get_activity_of_another_class()
    {
        // Arrange
        var director = await _back.LoggedAsDirector();
        var (classId, _) = await CreateClassWithTeacher(director);
        var (otherClassId, otherTeacherEmail) = await CreateClassWithTeacher(director);

        var otherTeacherClient = await _back.LoginAs(otherTeacherEmail);
        var otherActivity = await otherTeacherClient.CreateClassActivity(otherClassId).Success();

        var email = DataGen.Email;
        await EnrollStudentInClass(director, classId, email);

        var client = await _back.LoginAs(email);

        // Act
        var result = await client.GetStudentClassActivity(classId, otherActivity.Id);

        // Assert
        result.ShouldBeError(ClassActivityNotFound.I);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task Students_GetStudentClassActivity_Should_get_activity_with_pending_work()
    {
        // Arrange
        var director = await _back.LoggedAsDirector();
        var (classId, teacherEmail) = await CreateClassWithTeacher(director);

        var email = DataGen.Email;
        await EnrollStudentInClass(director, classId, email);

        var teacherClient = await _back.LoginAs(teacherEmail);
        var dueDate = DateTime.UtcNow.AddDays(7).ToDateOnly();
        var activity = (await teacherClient.CreateClassActivity(
            classId,
            ClassNoteType.N2,
            "Modelagem de Banco de Dados",
            "Modele um banco de dados para um sistema de gerenciamento de biblioteca.",
            ClassActivityType.Work,
            40,
            dueDate,
            Hour.H08_30
        )).Success;

        var client = await _back.LoginAs(email);

        // Act
        var result = await client.GetStudentClassActivity(classId, activity.Id);

        // Assert
        var item = result.Success;
        item.Id.Should().Be(activity.Id);
        item.ClassId.Should().Be(classId);
        item.Note.Should().Be(ClassNoteType.N2);
        item.Title.Should().Be("Modelagem de Banco de Dados");
        item.Description.Should().Be("Modele um banco de dados para um sistema de gerenciamento de biblioteca.");
        item.Type.Should().Be(ClassActivityType.Work);
        item.Status.Should().Be(ClassActivityStatus.Pending);
        item.Weight.Should().Be(40);
        item.DueDate.Should().Be(dueDate);
        item.DueHour.Should().Be(Hour.H08_30);
        item.WorkStatus.Should().Be(ClassActivityWorkStatus.Pending);
        item.WorkLink.Should().BeNull();
        item.Value.Should().Be(0);
        item.PonderedValue.Should().Be(0);
    }

    [Test]
    public async Task Students_GetStudentClassActivity_Should_get_activity_with_delivered_work()
    {
        // Arrange
        var director = await _back.LoggedAsDirector();
        var (classId, teacherEmail) = await CreateClassWithTeacher(director);

        var email = DataGen.Email;
        await EnrollStudentInClass(director, classId, email);

        var teacherClient = await _back.LoginAs(teacherEmail);
        var activity = await teacherClient.CreateClassActivity(classId, weight: 40).Success();

        var client = await _back.LoginAs(email);
        await client.CreateClassActivityWork(activity.Id, "https://github.com/ZaqueuCavalcante/estud");

        // Act
        var result = await client.GetStudentClassActivity(classId, activity.Id);

        // Assert
        var item = result.Success;
        item.Id.Should().Be(activity.Id);
        item.WorkStatus.Should().Be(ClassActivityWorkStatus.Delivered);
        item.WorkLink.Should().Be("https://github.com/ZaqueuCavalcante/estud");
    }

    #endregion
}
