namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Should_return_only_student_notifications()
    {
        // Arrange
        var academicClient = await _back.LoggedAsAcademic();
        var campus = await academicClient.CreateCampus("Agreste I", "Caruaru - PE");
        var period = await academicClient.CreateAcademicPeriod("2024.1");
        var course = await academicClient.CreateCourse("ADS");
        var courseCurriculum = await academicClient.CreateCourseCurriculum("Grade de ADS 1.0", course.Id);
        var courseOffering = await academicClient.CreateCourseOffering(campus.Id, course.Id, courseCurriculum.Id, period.Id, Shift.Noturno);

        var teacher = await academicClient.CreateTeacher("Chico");
        var student = await academicClient.CreateStudent(courseOffering.Id, "Zaqueu");

        await academicClient.CreateNotification("Hello", "Hi", UsersGroup.Students, true);

        var teacherClient = await _back.LoggedAsTeacher(teacher.Email);
        var studentClient = await _back.LoggedAsStudent(student.Email);

        // Act
        var teacherResponse = await teacherClient.GetUserNotifications();
        var studentResponse = await studentClient.Cross.GetUserNotifications();

        // Assert
        teacherResponse.Count.Should().Be(0);
        studentResponse.Count.Should().Be(1);
    }

    [Test]
    public async Task Should_return_only_teacher_notifications()
    {
        // Arrange
        var academicClient = await _back.LoggedAsAcademic();
        var campus = await academicClient.CreateCampus("Agreste I", "Caruaru - PE");
        var period = await academicClient.CreateAcademicPeriod("2024.1");
        var course = await academicClient.CreateCourse("ADS");
        var courseCurriculum = await academicClient.CreateCourseCurriculum("Grade de ADS 1.0", course.Id);
        var courseOffering = await academicClient.CreateCourseOffering(campus.Id, course.Id, courseCurriculum.Id, period.Id, Shift.Noturno);

        var teacher = await academicClient.CreateTeacher("Chico");
        var student = await academicClient.CreateStudent(courseOffering.Id, "Zaqueu");

        await academicClient.CreateNotification("Hello", "Hi", UsersGroup.Teachers, true);

        var teacherClient = await _back.LoggedAsTeacher(teacher.Email);
        var studentClient = await _back.LoggedAsStudent(student.Email);

        // Act
        var teacherResponse = await teacherClient.GetUserNotifications();
        var studentResponse = await studentClient.Cross.GetUserNotifications();

        // Assert
        teacherResponse.Count.Should().Be(1);
        studentResponse.Count.Should().Be(0);
    }

    [Test]
    public async Task Should_return_all_users_notifications()
    {
        // Arrange
        var academicClient = await _back.LoggedAsAcademic();
        var campus = await academicClient.CreateCampus("Agreste I", "Caruaru - PE");
        var period = await academicClient.CreateAcademicPeriod("2024.1");
        var course = await academicClient.CreateCourse("ADS");
        var courseCurriculum = await academicClient.CreateCourseCurriculum("Grade de ADS 1.0", course.Id);
        var courseOffering = await academicClient.CreateCourseOffering(campus.Id, course.Id, courseCurriculum.Id, period.Id, Shift.Noturno);

        var teacher = await academicClient.CreateTeacher("Chico");
        var student = await academicClient.CreateStudent(courseOffering.Id, "Zaqueu");

        await academicClient.CreateNotification("Hello", "Hi", UsersGroup.All, true);

        var teacherClient = await _back.LoggedAsTeacher(teacher.Email);
        var studentClient = await _back.LoggedAsStudent(student.Email);

        // Act
        var teacherResponse = await teacherClient.GetUserNotifications();
        var studentResponse = await studentClient.Cross.GetUserNotifications();

        // Assert
        teacherResponse.Count.Should().Be(1);
        studentResponse.Count.Should().Be(1);
    }

    [Test]
    public async Task Should_return_only_student_notifications_when_notification_is_timeless()
    {
        // Arrange
        var academicClient = await _back.LoggedAsAcademic();
        var campus = await academicClient.CreateCampus("Agreste I", "Caruaru - PE");
        var period = await academicClient.CreateAcademicPeriod("2024.1");
        var course = await academicClient.CreateCourse("ADS");
        var courseCurriculum = await academicClient.CreateCourseCurriculum("Grade de ADS 1.0", course.Id);
        var courseOffering = await academicClient.CreateCourseOffering(campus.Id, course.Id, courseCurriculum.Id, period.Id, Shift.Noturno);

        await academicClient.CreateNotification("Hello", "Hi", UsersGroup.Students, true);

        var teacher = await academicClient.CreateTeacher("Chico");
        var student = await academicClient.CreateStudent(courseOffering.Id, "Zaqueu");

        var teacherClient = await _back.LoggedAsTeacher(teacher.Email);
        var studentClient = await _back.LoggedAsStudent(student.Email);

        await _back.AwaitTasksProcessing();

        // Act
        var teacherResponse = await teacherClient.GetUserNotifications();
        var studentResponse = await studentClient.Cross.GetUserNotifications();

        // Assert
        teacherResponse.Count.Should().Be(0);
        studentResponse.Count.Should().Be(1);
    }

    [Test]
    public async Task Should_return_only_teacher_notifications_when_notification_is_timeless()
    {
        // Arrange
        var academicClient = await _back.LoggedAsAcademic();
        var campus = await academicClient.CreateCampus("Agreste I", "Caruaru - PE");
        var period = await academicClient.CreateAcademicPeriod("2024.1");
        var course = await academicClient.CreateCourse("ADS");
        var courseCurriculum = await academicClient.CreateCourseCurriculum("Grade de ADS 1.0", course.Id);
        var courseOffering = await academicClient.CreateCourseOffering(campus.Id, course.Id, courseCurriculum.Id, period.Id, Shift.Noturno);

        await academicClient.CreateNotification("Hello", "Hi", UsersGroup.Teachers, true);

        var teacher = await academicClient.CreateTeacher("Chico");
        var student = await academicClient.CreateStudent(courseOffering.Id, "Zaqueu");

        var teacherClient = await _back.LoggedAsTeacher(teacher.Email);
        var studentClient = await _back.LoggedAsStudent(student.Email);

        await _back.AwaitTasksProcessing();

        // Act
        var teacherResponse = await teacherClient.GetUserNotifications();
        var studentResponse = await studentClient.Cross.GetUserNotifications();

        // Assert
        teacherResponse.Count.Should().Be(1);
        studentResponse.Count.Should().Be(0);
    }

    [Test]
    public async Task Should_return_all_notifications_when_notification_is_timeless()
    {
        // Arrange
        var academicClient = await _back.LoggedAsAcademic();
        var campus = await academicClient.CreateCampus("Agreste I", "Caruaru - PE");
        var period = await academicClient.CreateAcademicPeriod("2024.1");
        var course = await academicClient.CreateCourse("ADS");
        var courseCurriculum = await academicClient.CreateCourseCurriculum("Grade de ADS 1.0", course.Id);
        var courseOffering = await academicClient.CreateCourseOffering(campus.Id, course.Id, courseCurriculum.Id, period.Id, Shift.Noturno);

        await academicClient.CreateNotification("Hello", "Hi", UsersGroup.All, true);

        var teacher = await academicClient.CreateTeacher("Chico");
        var student = await academicClient.CreateStudent(courseOffering.Id, "Zaqueu");

        var teacherClient = await _back.LoggedAsTeacher(teacher.Email);
        var studentClient = await _back.LoggedAsStudent(student.Email);

        await _back.AwaitTasksProcessing();

        // Act
        var teacherResponse = await teacherClient.GetUserNotifications();
        var studentResponse = await studentClient.Cross.GetUserNotifications();

        // Assert
        teacherResponse.Count.Should().Be(1);
        studentResponse.Count.Should().Be(1);
    }
}
