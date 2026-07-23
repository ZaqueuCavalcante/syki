using Estud.Back.Domain.Classes;
using Estud.Back.Features.Teachers.CreateClassActivity;

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
        var result = await client.CreateClassActivity(classId: 1);

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
        var result = await client.CreateClassActivity(classId: 1);

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
        var result = await client.CreateClassActivity(classId: 999999);

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
        var teacherClient = await _back.LoginAs(email);

        var discipline = await director.CreateDiscipline().Success();
        var otherTeacher = await director.CreateTeacher(DataGen.UserName, DataGen.Email).Success();
        await director.AssignDisciplinesToTeacher(otherTeacher.Id, [discipline.Id]);

        var period = await director.CreateAcademicPeriod().Success();
        var @class = await director.CreateClass(discipline.Id, period.Id).Success();

        // Act
        var result = await teacherClient.CreateClassActivity(@class.Id);

        // Assert
        result.ShouldBeError(TeacherNotAssignedToClass.I);
    }

    [TestCase(-1)]
    [TestCase(101)]
    public async Task Teachers_CreateClassActivity_Should_not_create_activity_with_invalid_weight(int weight)
    {
        // Arrange
        var director = await _back.LoggedAsDirector();

        var email = DataGen.Email;
        var teacher = await director.CreateTeacher(DataGen.UserName, email).Success();

        var discipline = await director.CreateDiscipline().Success();
        await director.AssignDisciplinesToTeacher(teacher.Id, [discipline.Id]);

        var period = await director.CreateAcademicPeriod().Success();
        var @class = await director.CreateClass(discipline.Id, period.Id).Success();
        await director.UpdateClassTeachers(@class.Id, [teacher.Id]);

        var client = await _back.LoginAs(email);

        // Act
        var result = await client.CreateClassActivity(@class.Id, weight: weight);

        // Assert
        result.ShouldBeError(InvalidClassActivityWeight.I);
    }

    [Test]
    public async Task Teachers_CreateClassActivity_Should_not_create_activity_when_note_weights_sum_exceeds_100()
    {
        // Arrange
        var director = await _back.LoggedAsDirector();

        var email = DataGen.Email;
        var teacher = await director.CreateTeacher(DataGen.UserName, email).Success();

        var discipline = await director.CreateDiscipline().Success();
        await director.AssignDisciplinesToTeacher(teacher.Id, [discipline.Id]);

        var period = await director.CreateAcademicPeriod().Success();
        var @class = await director.CreateClass(discipline.Id, period.Id).Success();
        await director.UpdateClassTeachers(@class.Id, [teacher.Id]);

        var client = await _back.LoginAs(email);
        await client.CreateClassActivity(@class.Id, ClassNoteType.N1, weight: 70);

        // Act
        var result = await client.CreateClassActivity(@class.Id, ClassNoteType.N1, weight: 31);

        // Assert
        result.ShouldBeError(InvalidClassActivityWeight.I);
    }

    #endregion

    #region Happy path

    [Test]
    public async Task Teachers_CreateClassActivity_Should_create_class_activity()
    {
        // Arrange
        var director = await _back.LoggedAsDirector();

        var email = DataGen.Email;
        var teacher = await director.CreateTeacher(DataGen.UserName, email).Success();

        var discipline = await director.CreateDiscipline().Success();
        await director.AssignDisciplinesToTeacher(teacher.Id, [discipline.Id]);

        var period = await director.CreateAcademicPeriod().Success();
        var @class = await director.CreateClass(discipline.Id, period.Id).Success();
        await director.UpdateClassTeachers(@class.Id, [teacher.Id]);

        var client = await _back.LoginAs(email);
        var dueDate = DateTime.UtcNow.AddDays(7).ToDateOnly();

        // Act
        var result = await client.CreateClassActivity(
            @class.Id,
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
        activity.ClassId.Should().Be(@class.Id);
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
        var director = await _back.LoggedAsDirector();

        var email = DataGen.Email;
        var teacher = await director.CreateTeacher(DataGen.UserName, email).Success();

        var discipline = await director.CreateDiscipline().Success();
        await director.AssignDisciplinesToTeacher(teacher.Id, [discipline.Id]);

        var period = await director.CreateAcademicPeriod().Success();
        var @class = await director.CreateClass(discipline.Id, period.Id).Success();
        await director.UpdateClassTeachers(@class.Id, [teacher.Id]);

        var client = await _back.LoginAs(email);

        // Act
        await client.CreateClassActivity(@class.Id, ClassNoteType.N1, type: ClassActivityType.Work, weight: 25);
        await client.CreateClassActivity(@class.Id, ClassNoteType.N1, type: ClassActivityType.Exam, weight: 75);
        await client.CreateClassActivity(@class.Id, ClassNoteType.N2, type: ClassActivityType.Presentation, weight: 40);
        await client.CreateClassActivity(@class.Id, ClassNoteType.N2, type: ClassActivityType.Exam, weight: 60);
        var result = await client.CreateClassActivity(@class.Id, ClassNoteType.N3, type: ClassActivityType.Project, weight: 100);

        // Assert
        result.Success.Id.Should().BeGreaterThan(0);

        await using var ctx = _back.GetDbContext();
        var activities = await ctx.ClassActivities.Where(x => x.ClassId == @class.Id).ToListAsync();
        activities.Should().HaveCount(5);
    }

    [Test]
    public async Task Teachers_CreateClassActivity_Should_notify_class_students_through_domain_event()
    {
        // Arrange
        var director = await _back.LoggedAsDirector();

        var teacherEmail = DataGen.Email;
        var teacher = await director.CreateTeacher(DataGen.UserName, teacherEmail).Success();

        var disciplineName = $"Modelagem de Dados {DataGen.Numbers}";
        var discipline = await director.CreateDiscipline(disciplineName).Success();
        await director.AssignDisciplinesToTeacher(teacher.Id, [discipline.Id]);

        var period = await director.CreateAcademicPeriod().Success();
        var @class = await director.CreateClass(discipline.Id, period.Id).Success();
        await director.UpdateClassTeachers(@class.Id, [teacher.Id]);

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        await director.CreateEnrollmentPeriod(startAt: today.AddDays(-2), endAt: today.AddDays(2));
        await director.ReleaseClassForEnrollment(@class.Id);

        var studentEmails = new List<string> { DataGen.Email, DataGen.Email, DataGen.Email };
        foreach (var studentEmail in studentEmails)
        {
            var student = await director.CreateStudent(DataGen.UserName, studentEmail).Success();
            await director.AssignStudentToClass(student.Id, @class.Id);
        }

        var client = await _back.LoginAs(teacherEmail);
        var title = $"Modelagem de Banco de Dados {DataGen.Numbers}";

        // Act
        var result = await client.CreateClassActivity(@class.Id, title: title);

        await _back.AwaitDomainEventsProcessing();
        await _back.AwaitCommandsProcessing();

        // Assert
        var activityId = result.Success.Id;

        await using var ctx = _back.GetDbContext();

        var activity = await ctx.ClassActivities.AsNoTracking().FirstAsync(x => x.Id == activityId);
        activity.Uid.Should().NotBeEmpty();

        var evt = await ctx.DomainEvents.AsNoTracking().SingleAsync(x => x.EntityUid == activity.Uid);
        evt.Type.Should().Be(typeof(ClassActivityCreated).FullName);
        evt.Error.Should().BeNull();
        evt.Status.Should().Be(DomainEventStatus.Success);
        evt.ProcessedAt.Should().NotBeNull();

        var commandData = new CreateNewClassActivityNotificationCommand(activityId).Serialize();
        var command = await ctx.Commands.AsNoTracking().SingleAsync(x => x.Data == commandData);
        command.Type.Should().Be(nameof(CreateNewClassActivityNotificationCommand));
        command.Error.Should().BeNull();
        command.Status.Should().Be(CommandStatus.Success);
        command.ProcessedAt.Should().NotBeNull();

        var notification = await ctx.Notifications.AsNoTracking()
            .SingleAsync(x => x.Description == $"{disciplineName}: {title}");
        notification.NotificationType.Should().Be(NotificationType.NewClassActivity);
        notification.Title.Should().Be("Nova atividade");

        var notifiedUserIds = await ctx.UserNotifications.AsNoTracking()
            .Where(x => x.NotificationId == notification.Id)
            .Select(x => x.UserId)
            .ToListAsync();

        var studentUserIds = await ctx.Students.AsNoTracking()
            .Where(x => studentEmails.Contains(x.User!.Email!))
            .Select(x => x.UserId)
            .ToListAsync();

        studentUserIds.Should().HaveCount(3);
        notifiedUserIds.Should().BeEquivalentTo(studentUserIds);
    }

    #endregion
}
