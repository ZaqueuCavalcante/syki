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

















    [Test]
    public async Task Deve_marcar_a_notificacao_como_vista_pelo_usuario()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademic();

        var campus = await client.CreateCampus("Agreste I", "Caruaru - PE");
        var periodo = await client.CreateAcademicPeriod("2024.1");
        var curso = await client.PostAsync<CourseOut>("/cursos", new CourseIn { Name = "ADS", Type = CourseType.Bacharelado });

        var grade = await client.PostAsync<GradeOut>("/grades", new GradeIn { Name = "Grade de ADS - 1.0", CursoId = curso.Id });
        var oferta = await client.PostAsync<OfertaOut>("/ofertas", new OfertaIn { CampusId = campus.Id, Periodo = periodo.Id, CursoId = curso.Id, GradeId = grade.Id });

        var bodyAluno = new AlunoIn { Name = "Zaqueu", Email = TestData.Email, OfertaId = oferta.Id };
        var aluno = await client.PostAsync<AlunoOut>("/alunos", bodyAluno);

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

    [Test]
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

    [Test]
    public async Task Deve_retornar_todas_as_notificacoes_do_usuario_logado()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademic();

        var campus = await client.CreateCampus("Agreste I", "Caruaru - PE");
        var periodo = await client.CreateAcademicPeriod("2024.1");
        var curso = await client.PostAsync<CourseOut>("/cursos", new CourseIn { Name = "ADS", Type = CourseType.Bacharelado });

        var grade = await client.PostAsync<GradeOut>("/grades", new GradeIn { Name = "Grade de ADS - 1.0", CursoId = curso.Id });
        var oferta = await client.PostAsync<OfertaOut>("/ofertas", new OfertaIn { CampusId = campus.Id, Periodo = periodo.Id, CursoId = curso.Id, GradeId = grade.Id });

        var bodyAluno = new AlunoIn { Name = "Zaqueu", Email = TestData.Email, OfertaId = oferta.Id };
        var aluno = await client.PostAsync<AlunoOut>("/alunos", bodyAluno);

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
