namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_return_only_student_notifications()
    {
        // Arrange
        var academicClient = await _api.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();

        TeacherOut teacher = await academicClient.CreateTeacher("Chico");
        StudentOut student = await academicClient.CreateStudent(data.AdsCourseOffering.Id, "Zaqueu");

        await academicClient.CreateNotification("Hello", "Hi", UsersGroup.Students, true);

        var teacherClient = await _api.LoggedAsTeacher(teacher.Email);
        var studentClient = await _api.LoggedAsStudent(student.Email);

        // Act
        var teacherResponse = await teacherClient.Cross.GetUserNotifications();
        var studentResponse = await studentClient.Http.GetUserNotifications();

        // Assert
        teacherResponse.Count.Should().Be(0);
        studentResponse.Count.Should().Be(1);
    }

    [Test]
    public async Task Should_return_only_teacher_notifications()
    {
        // Arrange
        var academicClient = await _api.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();

        TeacherOut teacher = await academicClient.CreateTeacher("Chico");
        StudentOut student = await academicClient.CreateStudent(data.AdsCourseOffering.Id, "Zaqueu");

        await academicClient.CreateNotification("Hello", "Hi", UsersGroup.Teachers, true);

        var teacherClient = await _api.LoggedAsTeacher(teacher.Email);
        var studentClient = await _api.LoggedAsStudent(student.Email);

        // Act
        var teacherResponse = await teacherClient.Cross.GetUserNotifications();
        var studentResponse = await studentClient.Http.GetUserNotifications();

        // Assert
        teacherResponse.Count.Should().Be(1);
        studentResponse.Count.Should().Be(0);
    }

    [Test]
    public async Task Should_return_all_users_notifications()
    {
        // Arrange
        var academicClient = await _api.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();

        TeacherOut teacher = await academicClient.CreateTeacher("Chico");
        StudentOut student = await academicClient.CreateStudent(data.AdsCourseOffering.Id, "Zaqueu");

        await academicClient.CreateNotification("Hello", "Hi", UsersGroup.All, true);

        var teacherClient = await _api.LoggedAsTeacher(teacher.Email);
        var studentClient = await _api.LoggedAsStudent(student.Email);

        // Act
        var teacherResponse = await teacherClient.Cross.GetUserNotifications();
        var studentResponse = await studentClient.Http.GetUserNotifications();

        // Assert
        teacherResponse.Count.Should().Be(1);
        studentResponse.Count.Should().Be(1);
    }

    [Test]
    public async Task Should_return_only_student_notifications_when_notification_is_timeless()
    {
        // Arrange
        var academicClient = await _api.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();

        await academicClient.CreateNotification("Hello", "Hi", UsersGroup.Students, true);

        TeacherOut teacher = await academicClient.CreateTeacher("Chico");
        StudentOut student = await academicClient.CreateStudent(data.AdsCourseOffering.Id, "Zaqueu");

        var teacherClient = await _api.LoggedAsTeacher(teacher.Email);
        var studentClient = await _api.LoggedAsStudent(student.Email);

        // await _api.AwaitTasksProcessing();

        // Act
        var teacherResponse = await teacherClient.Cross.GetUserNotifications();
        var studentResponse = await studentClient.Http.GetUserNotifications();

        // Assert
        teacherResponse.Count.Should().Be(0);
        studentResponse.Count.Should().Be(1);
    }

    [Test]
    public async Task Should_return_only_teacher_notifications_when_notification_is_timeless()
    {
        // Arrange
        var academicClient = await _api.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();

        await academicClient.CreateNotification("Hello", "Hi", UsersGroup.Teachers, true);

        TeacherOut teacher = await academicClient.CreateTeacher("Chico");
        StudentOut student = await academicClient.CreateStudent(data.AdsCourseOffering.Id, "Zaqueu");

        var teacherClient = await _api.LoggedAsTeacher(teacher.Email);
        var studentClient = await _api.LoggedAsStudent(student.Email);

        // await _api.AwaitTasksProcessing();

        // Act
        var teacherResponse = await teacherClient.Cross.GetUserNotifications();
        var studentResponse = await studentClient.Http.GetUserNotifications();

        // Assert
        teacherResponse.Count.Should().Be(1);
        studentResponse.Count.Should().Be(0);
    }

    [Test]
    public async Task Should_return_all_notifications_when_notification_is_timeless()
    {
        // Arrange
        var academicClient = await _api.LoggedAsAcademic();
        var data = await academicClient.CreateBasicInstitutionData();

        await academicClient.CreateNotification("Hello", "Hi", UsersGroup.All, true);

        TeacherOut teacher = await academicClient.CreateTeacher("Chico");
        StudentOut student = await academicClient.CreateStudent(data.AdsCourseOffering.Id, "Zaqueu");

        var teacherClient = await _api.LoggedAsTeacher(teacher.Email);
        var studentClient = await _api.LoggedAsStudent(student.Email);

        // await _api.AwaitTasksProcessing();

        // Act
        var teacherResponse = await teacherClient.Cross.GetUserNotifications();
        var studentResponse = await studentClient.Http.GetUserNotifications();

        // Assert
        teacherResponse.Count.Should().Be(1);
        studentResponse.Count.Should().Be(1);
    }
}
