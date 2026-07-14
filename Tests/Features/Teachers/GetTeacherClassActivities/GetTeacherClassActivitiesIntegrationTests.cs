namespace Estud.Tests.Integration;

public partial class IntegrationTests
{
    #region Authentication

    [Test]
    public async Task Teachers_GetTeacherClassActivities_Should_not_get_activities_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.GetTeacherClassActivities(1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Authorization

    [Test]
    public async Task Teachers_GetTeacherClassActivities_Should_not_get_activities_when_user_is_not_a_teacher()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.GetTeacherClassActivities(1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    #endregion

    #region Validation errors

    [Test]
    public async Task Teachers_GetTeacherClassActivities_Should_not_get_activities_when_class_not_found()
    {
        // Arrange
        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.GetTeacherClassActivities(999999);

        // Assert
        result.ShouldBeError(ClassNotFound.I);
    }

    [Test]
    public async Task Teachers_GetTeacherClassActivities_Should_not_get_activities_of_class_of_another_institution()
    {
        // Arrange
        var director = await _back.LoggedAsDirector();
        var discipline = (await director.CreateDiscipline()).Success;
        var period = (await director.CreateAcademicPeriod()).Success;
        var @class = (await director.CreateClass(discipline.Id, period.Id)).Success;

        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.GetTeacherClassActivities(@class.Id);

        // Assert
        result.ShouldBeError(ClassNotFound.I);
    }

    [Test]
    public async Task Teachers_GetTeacherClassActivities_Should_not_get_activities_of_class_of_another_teacher()
    {
        // Arrange
        var director = await _back.LoggedAsDirector();

        var email = DataGen.Email;
        await director.CreateTeacher(DataGen.UserName, email);
        var otherTeacher = (await director.CreateTeacher(DataGen.UserName, DataGen.Email)).Success;

        var discipline = (await director.CreateDiscipline()).Success;
        await director.AssignDisciplinesToTeacher(otherTeacher.Id, [discipline.Id]);

        var period = (await director.CreateAcademicPeriod()).Success;
        var @class = (await director.CreateClass(discipline.Id, period.Id, teacherId: otherTeacher.Id)).Success;

        var client = await _back.LoginAs(email);

        // Act
        var result = await client.GetTeacherClassActivities(@class.Id);

        // Assert
        result.ShouldBeError(TeacherNotAssignedToClass.I);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task Teachers_GetTeacherClassActivities_Should_get_empty_list_when_class_has_no_activities()
    {
        // Arrange
        var email = DataGen.Email;
        var @class = await CreateTeacherClass(email);

        var client = await _back.LoginAs(email);

        // Act
        var result = await client.GetTeacherClassActivities(@class);

        // Assert
        result.Success.Activities.Should().BeEmpty();
    }

    [Test]
    public async Task Teachers_GetTeacherClassActivities_Should_get_class_activities()
    {
        // Arrange
        var email = DataGen.Email;
        var @class = await CreateTeacherClass(email);

        var client = await _back.LoginAs(email);

        var dueDate = DateTime.UtcNow.AddDays(7).ToDateOnly();
        await client.CreateClassActivity(
            @class,
            ClassNoteType.N1,
            "Modelagem de Banco de Dados",
            "Modele um banco de dados para um sistema de gerenciamento de biblioteca.",
            ClassActivityType.Work,
            40,
            dueDate,
            Hour.H08_30
        );

        // Act
        var result = await client.GetTeacherClassActivities(@class);

        // Assert
        var activities = result.Success.Activities;
        activities.Should().ContainSingle();

        var activity = activities[0];
        activity.Id.Should().BeGreaterThan(0);
        activity.ClassId.Should().Be(@class);
        activity.Note.Should().Be(ClassNoteType.N1);
        activity.Title.Should().Be("Modelagem de Banco de Dados");
        activity.Description.Should().Be("Modele um banco de dados para um sistema de gerenciamento de biblioteca.");
        activity.Type.Should().Be(ClassActivityType.Work);
        activity.Status.Should().Be(ClassActivityStatus.Pending);
        activity.Weight.Should().Be(40);
        activity.DueDate.Should().Be(dueDate);
        activity.DueHour.Should().Be(Hour.H08_30);
        activity.DeliveredWorks.Should().Be(0);
        activity.TotalWorks.Should().Be(0);
    }

    [Test]
    public async Task Teachers_GetTeacherClassActivities_Should_get_class_activities_ordered_by_note()
    {
        // Arrange
        var email = DataGen.Email;
        var @class = await CreateTeacherClass(email);

        var client = await _back.LoginAs(email);

        await client.CreateClassActivity(@class, ClassNoteType.N3, type: ClassActivityType.Project, weight: 100);
        await client.CreateClassActivity(@class, ClassNoteType.N1, type: ClassActivityType.Work, weight: 25);
        await client.CreateClassActivity(@class, ClassNoteType.N2, type: ClassActivityType.Exam, weight: 60);

        // Act
        var result = await client.GetTeacherClassActivities(@class);

        // Assert
        var activities = result.Success.Activities;
        activities.Should().HaveCount(3);
        activities.Select(x => x.Note).Should().ContainInOrder(ClassNoteType.N1, ClassNoteType.N2, ClassNoteType.N3);
    }

    [Test]
    public async Task Teachers_GetTeacherClassActivities_Should_get_activities_works_count()
    {
        // Arrange
        var director = await _back.LoggedAsDirector();

        var email = DataGen.Email;
        var teacher = (await director.CreateTeacher(DataGen.UserName, email)).Success;

        var discipline = (await director.CreateDiscipline()).Success;
        await director.AssignDisciplinesToTeacher(teacher.Id, [discipline.Id]);

        var period = (await director.CreateAcademicPeriod()).Success;
        var @class = (await director.CreateClass(discipline.Id, period.Id, teacherId: teacher.Id)).Success;

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        await director.CreateEnrollmentPeriod(startAt: today.AddDays(-2), endAt: today.AddDays(2));
        await director.ReleaseClassForEnrollment(@class.Id);

        var student = (await director.CreateStudent(DataGen.UserName, DataGen.Email)).Success;
        await director.AssignStudentToClass(student.Id, @class.Id);

        var client = await _back.LoginAs(email);
        await client.CreateClassActivity(@class.Id);

        // Act
        var result = await client.GetTeacherClassActivities(@class.Id);

        // Assert
        var activities = result.Success.Activities;
        activities.Should().ContainSingle();
        activities[0].TotalWorks.Should().Be(1);
        activities[0].DeliveredWorks.Should().Be(0);
    }

    #endregion
}
