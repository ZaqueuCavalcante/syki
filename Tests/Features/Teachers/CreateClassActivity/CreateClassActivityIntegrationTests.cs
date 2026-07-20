namespace Estud.Tests.Integration;

public partial class IntegrationTests
{
    #region Authentication

    [Test]
    public async Task Teachers_CreateClassActivity_Should_not_create_activity_when_not_authenticated()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.CreateClassActivity(1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Authorization

    [Test]
    public async Task Teachers_CreateClassActivity_Should_not_create_activity_when_user_is_not_a_teacher()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.CreateClassActivity(1);

        // Assert
        result.ShouldBeError(HttpStatusCode.Forbidden);
    }

    #endregion

    #region Validation errors

    [Test]
    public async Task Teachers_CreateClassActivity_Should_not_create_activity_when_class_not_found()
    {
        // Arrange
        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.CreateClassActivity(999999);

        // Assert
        result.ShouldBeError(ClassNotFound.I);
    }

    [Test]
    public async Task Teachers_CreateClassActivity_Should_not_create_activity_on_class_of_another_institution()
    {
        // Arrange
        var director = await _back.LoggedAsDirector();
        var discipline = await director.CreateDiscipline().Success();
        var period = await director.CreateAcademicPeriod().Success();
        var @class = await director.CreateClass(discipline.Id, period.Id).Success();

        var client = await _back.LoggedAsTeacher();

        // Act
        var result = await client.CreateClassActivity(@class.Id);

        // Assert
        result.ShouldBeError(ClassNotFound.I);
    }

    [Test]
    public async Task Teachers_CreateClassActivity_Should_not_create_activity_on_class_of_another_teacher()
    {
        // Arrange
        var director = await _back.LoggedAsDirector();

        var email = DataGen.Email;
        await director.CreateTeacher(DataGen.UserName, email);
        var otherTeacher = await director.CreateTeacher(DataGen.UserName, DataGen.Email).Success();

        var discipline = await director.CreateDiscipline().Success();
        await director.AssignDisciplinesToTeacher(otherTeacher.Id, [discipline.Id]);

        var period = await director.CreateAcademicPeriod().Success();
        var @class = await director.CreateClass(discipline.Id, period.Id).Success();

        var client = await _back.LoginAs(email);

        // Act
        var result = await client.CreateClassActivity(@class.Id);

        // Assert
        result.ShouldBeError(TeacherNotAssignedToClass.I);
    }

    [TestCase(-1)]
    [TestCase(101)]
    public async Task Teachers_CreateClassActivity_Should_not_create_activity_with_invalid_weight(int weight)
    {
        // Arrange
        var email = DataGen.Email;
        var @class = await CreateTeacherClass(email);

        var client = await _back.LoginAs(email);

        // Act
        var result = await client.CreateClassActivity(@class, weight: weight);

        // Assert
        result.ShouldBeError(InvalidClassActivityWeight.I);
    }

    [Test]
    public async Task Teachers_CreateClassActivity_Should_not_create_activity_when_note_weights_sum_exceeds_100()
    {
        // Arrange
        var email = DataGen.Email;
        var @class = await CreateTeacherClass(email);

        var client = await _back.LoginAs(email);
        await client.CreateClassActivity(@class, ClassNoteType.N1, weight: 70);

        // Act
        var result = await client.CreateClassActivity(@class, ClassNoteType.N1, weight: 31);

        // Assert
        result.ShouldBeError(InvalidClassActivityWeight.I);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task Teachers_CreateClassActivity_Should_create_activity()
    {
        // Arrange
        var email = DataGen.Email;
        var @class = await CreateTeacherClass(email);

        var client = await _back.LoginAs(email);
        var dueDate = DateTime.UtcNow.AddDays(7).ToDateOnly();

        // Act
        var result = await client.CreateClassActivity(
            @class,
            ClassNoteType.N2,
            "Modelagem de Banco de Dados",
            "Modele um banco de dados para um sistema de gerenciamento de biblioteca.",
            ClassActivityType.Work,
            69,
            dueDate,
            Hour.H08_30
        );

        // Assert
        var activityId = result.Success.Id;
        activityId.Should().BeGreaterThan(0);

        await using var ctx = _back.GetDbContext();
        var activity = await ctx.ClassActivities.FirstAsync(x => x.Id == activityId);
        activity.ClassId.Should().Be(@class);
        activity.Note.Should().Be(ClassNoteType.N2);
        activity.Title.Should().Be("Modelagem de Banco de Dados");
        activity.Description.Should().Be("Modele um banco de dados para um sistema de gerenciamento de biblioteca.");
        activity.ActivityType.Should().Be(ClassActivityType.Work);
        activity.Status.Should().Be(ClassActivityStatus.Pending);
        activity.Weight.Should().Be(69);
        activity.DueDate.Should().Be(dueDate);
        activity.DueHour.Should().Be(Hour.H08_30);
    }

    [Test]
    public async Task Teachers_CreateClassActivity_Should_create_activities_with_valid_weights_on_each_note()
    {
        // Arrange
        var email = DataGen.Email;
        var @class = await CreateTeacherClass(email);

        var client = await _back.LoginAs(email);

        // Act
        await client.CreateClassActivity(@class, ClassNoteType.N1, type: ClassActivityType.Work, weight: 25);
        await client.CreateClassActivity(@class, ClassNoteType.N1, type: ClassActivityType.Exam, weight: 75);
        await client.CreateClassActivity(@class, ClassNoteType.N2, type: ClassActivityType.Presentation, weight: 40);
        await client.CreateClassActivity(@class, ClassNoteType.N2, type: ClassActivityType.Exam, weight: 60);
        var result = await client.CreateClassActivity(@class, ClassNoteType.N3, type: ClassActivityType.Project, weight: 100);

        // Assert
        result.Success.Id.Should().BeGreaterThan(0);

        await using var ctx = _back.GetDbContext();
        var activities = await ctx.ClassActivities.Where(x => x.ClassId == @class).ToListAsync();
        activities.Should().HaveCount(5);
    }

    [Test]
    public async Task Teachers_CreateClassActivity_Should_create_pending_works_for_enrolled_students()
    {
        // Arrange
        var director = await _back.LoggedAsDirector();

        var email = DataGen.Email;
        var teacher = await director.CreateTeacher(DataGen.UserName, email).Success();

        var discipline = await director.CreateDiscipline().Success();
        await director.AssignDisciplinesToTeacher(teacher.Id, [discipline.Id]);

        var period = await director.CreateAcademicPeriod().Success();
        var @class = await director.CreateClass(discipline.Id, period.Id).Success();

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        await director.CreateEnrollmentPeriod(startAt: today.AddDays(-2), endAt: today.AddDays(2));
        await director.ReleaseClassForEnrollment(@class.Id);

        var student = await director.CreateStudent(DataGen.UserName, DataGen.Email).Success();
        await director.AssignStudentToClass(student.Id, @class.Id);

        var client = await _back.LoginAs(email);

        // Act
        var result = await client.CreateClassActivity(@class.Id);

        // Assert
        var activityId = result.Success.Id;

        await using var ctx = _back.GetDbContext();
        var works = await ctx.ClassActivityWorks.Where(x => x.ClassActivityId == activityId).ToListAsync();
        works.Should().ContainSingle();
        works[0].StudentId.Should().Be(student.Id);
        works[0].Status.Should().Be(ClassActivityWorkStatus.Pending);
        works[0].Note.Should().Be(0);
    }

    #endregion

    private async Task<int> CreateTeacherClass(string teacherEmail)
    {
        var director = await _back.LoggedAsDirector();

        var teacher = await director.CreateTeacher(DataGen.UserName, teacherEmail).Success();

        var discipline = await director.CreateDiscipline().Success();
        await director.AssignDisciplinesToTeacher(teacher.Id, [discipline.Id]);

        var period = await director.CreateAcademicPeriod().Success();
        var @class = await director.CreateClass(discipline.Id, period.Id).Success();
        await director.AssignTeachersToClass(@class.Id, [teacher.Id]);

        return @class.Id;
    }
}
