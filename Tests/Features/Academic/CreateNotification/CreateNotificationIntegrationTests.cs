namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Should_create_a_notification_without_target_users()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademic();

        // Act
        var response = await client.CreateNotification("Hello", "Hi", UsersGroup.All);

        // Assert
        response.Id.Should().NotBeEmpty();
        response.Title.Should().Be("Hello");
        response.Description.Should().Be("Hi");
    }

















    [Test, Ignore("")]
    public async Task Deve_marcar_a_notificacao_como_vista_pelo_usuario()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademic();

        var campus = await client.CreateCampus("Agreste I", "Caruaru - PE");
        var period = await client.CreateAcademicPeriod("2024.1");
        var course = await client.PostAsync<CourseOut>("/cursos", new CreateCourseIn { Name = "ADS", Type = CourseType.Bacharelado });

        var grade = await client.PostAsync<CourseCurriculumOut>("/grades", new CreateCourseCurriculumIn { Name = "Grade de ADS - 1.0", CourseId = course.Id });
        var oferta = await client.PostAsync<CourseOfferingOut>("/ofertas", new CreateCourseOfferingIn { CampusId = campus.Id, Period = period.Id, CourseId = course.Id, CourseCurriculumId = grade.Id });

        var bodyAluno = new CreateStudentIn { Name = "Zaqueu", Email = TestData.Email, CourseOfferingId = oferta.Id };
        var aluno = await client.PostAsync<StudentOut>("/alunos", bodyAluno);

        var body = new CreateNotificationIn("Hello", "Hi", UsersGroup.Students);
        await client.PostAsync<NotificationOut>("/notifications", body);

        // Act
        var token = await _factory.GetResetPasswordToken(aluno.Email);
        var password = await client.ResetPassword(token!);
        await client.Login(bodyAluno.Email, password);
        var response = await client.PutAsync("/notifications/user", null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        using var ctx = _factory.GetDbContext();
        var notification = await ctx.UserNotifications.FirstAsync(n => n.UserId == aluno.Id);
        notification.ViewedAt.Should().NotBeNull();
    }

    [Test, Ignore("")]
    public async Task Deve_retornar_todas_as_notificacoes()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademic();

        await client.CreateNotification("Hello", "Hi", UsersGroup.Students);
        await client.CreateNotification("Olá", "Olá", UsersGroup.Teachers);

        // Act
        var notifications = await client.GetAsync<List<NotificationOut>>("/notifications");

        // Assert
        notifications.Count.Should().Be(2);
    }

    [Test, Ignore("")]
    public async Task Deve_retornar_todas_as_notificacoes_do_usuario_logado()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademic();

        var campus = await client.CreateCampus("Agreste I", "Caruaru - PE");
        var period = await client.CreateAcademicPeriod("2024.1");
        var course = await client.PostAsync<CourseOut>("/cursos", new CreateCourseIn { Name = "ADS", Type = CourseType.Bacharelado });

        var grade = await client.PostAsync<CourseCurriculumOut>("/grades", new CreateCourseCurriculumIn { Name = "Grade de ADS - 1.0", CourseId = course.Id });
        var oferta = await client.PostAsync<CourseOfferingOut>("/ofertas", new CreateCourseOfferingIn { CampusId = campus.Id, Period = period.Id, CourseId = course.Id, CourseCurriculumId = grade.Id });

        var bodyAluno = new CreateStudentIn { Name = "Zaqueu", Email = TestData.Email, CourseOfferingId = oferta.Id };
        var aluno = await client.PostAsync<StudentOut>("/alunos", bodyAluno);

        await client.CreateNotification("Hello", "Hi", UsersGroup.Students);

        // Act
        var token = await _factory.GetResetPasswordToken(aluno.Email);
        var password = await client.ResetPassword(token!);
        await client.Login(bodyAluno.Email, password);
        var response = await client.GetAsync<List<UserNotificationOut>>("/notifications/user");

        // Assert
        response.Count.Should().Be(1);
    }
}
